// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Security.Authentication;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AppCoreNet.Diagnostics;
using IdentityModel;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AppCoreNet.Extensions.Http.Authentication.OAuth.AspNetCore;

/// <summary>
/// Provides the base class for see <see cref="IOAuthUserTokenService"/>.
/// </summary>
/// <typeparam name="TOptions">The type of the <see cref="OAuthUserOptions"/>.</typeparam>
public abstract class OAuthUserTokenService<TOptions> : IOAuthUserTokenService
    where TOptions : OAuthUserOptions
{
    private static readonly ConcurrentDictionary<string, Lazy<Task<OAuthUserToken>>> _sync = new ();
    private readonly IOAuthTokenClient _client;
    private readonly IOAuthUserTokenStore _store;
    private readonly TimeProvider _timeProvider;
    private readonly IOptionsMonitor<TOptions> _optionsMonitor;
    private readonly ILogger _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="OAuthUserTokenService{TOptions}"/> class.
    /// </summary>
    /// <param name="client">The <see cref="IOAuthTokenClient"/>.</param>
    /// <param name="store">The <see cref="IOAuthUserTokenStore"/>.</param>
    /// <param name="timeProvider">The <see cref="TimeProvider"/>.</param>
    /// <param name="optionsMonitor">The <see cref="IOptionsMonitor{TOptions}"/>. </param>
    /// <param name="logger">The <see cref="ILogger"/>.</param>
    protected OAuthUserTokenService(
        IOAuthTokenClient client,
        IOAuthUserTokenStore store,
        TimeProvider timeProvider,
        IOptionsMonitor<TOptions> optionsMonitor,
        ILogger logger)
    {
        Ensure.Arg.NotNull(client);
        Ensure.Arg.NotNull(store);
        Ensure.Arg.NotNull(timeProvider);
        Ensure.Arg.NotNull(optionsMonitor);
        Ensure.Arg.NotNull(logger);

        _client = client;
        _store = store;
        _timeProvider = timeProvider;
        _optionsMonitor = optionsMonitor;
        _logger = logger;
    }

    /// <summary>
    /// Ensures that the <paramref name="scheme"/> is compatible.
    /// </summary>
    /// <param name="scheme">The <see cref="AuthenticationScheme"/>.</param>
    protected abstract void EnsureCompatibleScheme(AuthenticationScheme scheme);

    [SuppressMessage("Usage", "VSTHRD011:Use AsyncLazy<T>", Justification = "ConfigureAwait(false) is used")]
    private async Task<OAuthUserToken> InvokeSynchronizedAsync(string key, Func<Task<OAuthUserToken>> tokenFunc)
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

        TOptions options = _optionsMonitor.Get(scheme.Name);
        OAuthUserToken token = await _store.GetTokenAsync(scheme, user, cancellationToken);

        DateTimeOffset? refreshAt = token.Expires?.Subtract(options.RefreshBeforeExpiration);
        if ((refreshAt.HasValue && refreshAt < _timeProvider.GetUtcNow())
            || (parameters?.ForceRenewal).GetValueOrDefault())
        {
            if (!options.AllowTokenRefresh)
                throw new AuthenticationException("Cannot refresh access token because it has been disabled.");

            if (string.IsNullOrWhiteSpace(token.RefreshToken))
                throw new AuthenticationException("Cannot refresh access token because no refresh token was found for user.");

            return await InvokeSynchronizedAsync(
                    token.RefreshToken,
                    () => RefreshAccessTokenAsync(scheme, user, token, parameters, cancellationToken))
                .ConfigureAwait(false);
        }

        return token;
    }

    private async Task<OAuthUserToken> RefreshAccessTokenAsync(
        AuthenticationScheme scheme,
        ClaimsPrincipal user,
        OAuthUserToken token,
        OAuthUserParameters? parameters = null,
        CancellationToken cancellationToken = default)
    {
        _logger.LogDebug("Refreshing access token for client scheme {SchemeName} ...", scheme.Name);

        TokenResponse response =
            await _client.RequestRefreshTokenAsync(
                             scheme,
                             token.RefreshToken!,
                             parameters,
                             cancellationToken)
                         .ConfigureAwait(false);

        if (response.IsError)
        {
            _logger.LogError(
                "Error refreshing access token for client scheme {SchemeName}. Error = {Error}. Error description = {ErrorDescription}",
                scheme.Name,
                response.Error,
                response.ErrorDescription);

            throw new AuthenticationException(
                $"Error refreshing access token for client scheme '{scheme.Name}': {response.Error}");
        }

        OAuthUserToken refreshedToken = new (
            response.AccessToken,
            response.RefreshToken,
            response.ExpiresIn > 0 ? _timeProvider.GetUtcNow() + TimeSpan.FromSeconds(response.ExpiresIn) : null);

        _logger.LogDebug(
            "Refreshed access token for client scheme {SchemeName}. Expiration: {Expiration}",
            scheme.Name,
            refreshedToken.Expires);

        await _store.StoreTokenAsync(scheme, user, refreshedToken, cancellationToken)
                    .ConfigureAwait(false);

        return refreshedToken;
    }

    /// <inheritdoc />
    public async Task RevokeRefreshTokenAsync(
        AuthenticationScheme scheme,
        ClaimsPrincipal user,
        CancellationToken cancellationToken = default)
    {
        Ensure.Arg.NotNull(scheme);
        Ensure.Arg.NotNull(user);

        EnsureCompatibleScheme(scheme);

        OAuthUserToken token = await _store.GetTokenAsync(scheme, user, cancellationToken)
                                           .ConfigureAwait(false);

        if (!string.IsNullOrWhiteSpace(token.RefreshToken))
        {
            await _client.RevokeTokenAsync(
                             scheme,
                             token.RefreshToken,
                             OidcConstants.TokenTypeIdentifiers.RefreshToken,
                             cancellationToken)
                         .ConfigureAwait(false);

            await _store.ClearTokenAsync(scheme, user, cancellationToken)
                        .ConfigureAwait(false);
        }
    }
}