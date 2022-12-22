using System;
using System.Collections.Concurrent;
using System.Security.Authentication;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AppCore.Diagnostics;
using IdentityModel;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AppCore.Extensions.Http.Authentication.OAuth.AspNetCore;

public abstract class OAuthUserTokenService<TOptions> : IOAuthUserTokenService
    where TOptions : OAuthUserOptions
{
    private static readonly ConcurrentDictionary<string, Lazy<Task<OAuthUserToken>>> _sync = new();
    private readonly IOAuthTokenClient _client;
    private readonly IOAuthUserTokenStore _store;
    private readonly ISystemClock _clock;
    private readonly IOptionsMonitor<TOptions> _optionsMonitor;
    private readonly ILogger _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="OAuthUserTokenService{TOptions}"/> class.
    /// </summary>
    /// <param name="client"></param>
    /// <param name="store"></param>
    /// <param name="clock"></param>
    /// <param name="optionsMonitor"></param>
    /// <param name="logger"></param>
    protected OAuthUserTokenService(
        IOAuthTokenClient client,
        IOAuthUserTokenStore store,
        ISystemClock clock,
        IOptionsMonitor<TOptions> optionsMonitor,
        ILogger logger)
    {
        Ensure.Arg.NotNull(client);
        Ensure.Arg.NotNull(store);
        Ensure.Arg.NotNull(clock);
        Ensure.Arg.NotNull(optionsMonitor);
        Ensure.Arg.NotNull(logger);

        _client = client;
        _store = store;
        _clock = clock;
        _optionsMonitor = optionsMonitor;
        _logger = logger;
    }

    protected abstract void EnsureCompatibleScheme(AuthenticationScheme scheme);

    private async Task<OAuthUserToken> InvokeSynchronized(string key, Func<Task<OAuthUserToken>> tokenFunc)
    {
        try
        {
            return await _sync.GetOrAdd(
                                  key,
                                  _ => new Lazy<Task<OAuthUserToken>>(tokenFunc))
                              .Value.ConfigureAwait(false);
        }
        finally
        {
            _sync.TryRemove(key, out _);
        }
    }

    /// <inheritdoc />
    public async Task<OAuthUserToken> GetAccessTokenAsync(
        AuthenticationScheme scheme,
        ClaimsPrincipal user,
        OAuthUserParameters? parameters = null,
        CancellationToken cancellationToken = default)
    {
        Ensure.Arg.NotNull(scheme);
        Ensure.Arg.NotNull(user);

        EnsureCompatibleScheme(scheme);

        OAuthUserOptions options = _optionsMonitor.Get(scheme.Name);
        OAuthUserToken token = await _store.GetTokenAsync(scheme, user, parameters, cancellationToken);

        if (!options.AllowTokenRefresh)
            return token;

        DateTimeOffset? refreshAt = token.Expires?.Subtract(options.RefreshBeforeExpiration);
        if (refreshAt.HasValue
            && refreshAt < _clock.UtcNow
            || (parameters?.ForceRenewal).GetValueOrDefault())
        {
            if (string.IsNullOrWhiteSpace(token.RefreshToken))
                throw new AuthenticationException("Cannot refresh access token because no refresh token was found for user.");

            return await InvokeSynchronized(
                    token.RefreshToken,
                    async () =>
                    {
                        _logger.LogDebug("Refreshing access token for client scheme {schemeName} ...", scheme.Name);

                        TokenResponse response =
                            await _client.RequestRefreshTokenAsync(
                                             scheme,
                                             token.RefreshToken,
                                             parameters,
                                             cancellationToken)
                                         .ConfigureAwait(false);

                        if (response.IsError)
                        {
                            _logger.LogError(
                                "Error refreshing access token for client scheme {schemeName}. Error = {error}. Error description = {errorDescription}",
                                scheme.Name,
                                response.Error,
                                response.ErrorDescription);

                            throw new AuthenticationException(
                                $"Error refreshing access token for client scheme '{scheme.Name}': {response.Error}");
                        }

                        OAuthUserToken refreshedToken = new(
                            response.AccessToken,
                            response.RefreshToken,
                            response.ExpiresIn > 0
                                ? DateTimeOffset.UtcNow + TimeSpan.FromSeconds(response.ExpiresIn)
                                : null);

                        _logger.LogDebug(
                            "Refreshed access token for client scheme {schemeName}. Expiration: {expiration}",
                            scheme.Name,
                            refreshedToken.Expires);

                        await _store.StoreTokenAsync(scheme, user, refreshedToken, parameters, cancellationToken)
                                    .ConfigureAwait(false);

                        return refreshedToken;
                    })
                .ConfigureAwait(false);
        }

        return token;
    }

    /// <inheritdoc />
    public async Task RevokeRefreshTokenAsync(
        AuthenticationScheme scheme,
        ClaimsPrincipal user,
        OAuthUserParameters? parameters = null,
        CancellationToken cancellationToken = default)
    {
        Ensure.Arg.NotNull(scheme);
        Ensure.Arg.NotNull(user);

        EnsureCompatibleScheme(scheme);

        OAuthUserToken token = await _store.GetTokenAsync(scheme, user, parameters, cancellationToken)
                                           .ConfigureAwait(false);

        if (!string.IsNullOrWhiteSpace(token.RefreshToken))
        {
            await _client.RevokeTokenAsync(
                             scheme,
                             token.RefreshToken,
                             OidcConstants.TokenTypeIdentifiers.RefreshToken,
                             parameters,
                             cancellationToken)
                         .ConfigureAwait(false);

            await _store.ClearTokenAsync(scheme, user, parameters, cancellationToken)
                        .ConfigureAwait(false);
        }
    }
}