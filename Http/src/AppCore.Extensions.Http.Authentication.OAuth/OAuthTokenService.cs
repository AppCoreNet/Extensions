// Licensed under the MIT License.
// Copyright (c) 2018-2022 the AppCore .NET project.

using System;
using System.Collections.Concurrent;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;
using AppCore.Diagnostics;
using IdentityModel.Client;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Logging;

namespace AppCore.Extensions.Http.Authentication.OAuth;

/// <summary>
/// Service for getting OAuth access tokens.
/// </summary>
public class OAuthTokenService : IOAuthTokenService
{
    private static readonly ConcurrentDictionary<string, Lazy<Task<OAuthAccessToken>>> _sync = new();
    private readonly IOAuthTokenClient _client;
    private readonly IOAuthTokenCache _cache;
    private readonly ISystemClock _clock;
    private readonly ILogger<OAuthTokenService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="OAuthTokenService"/> class.
    /// </summary>
    /// <param name="client"></param>
    /// <param name="cache"></param>
    /// <param name="clock"></param>
    /// <param name="logger"></param>
    public OAuthTokenService(IOAuthTokenClient client, IOAuthTokenCache cache, ISystemClock clock, ILogger<OAuthTokenService> logger)
    {
        Ensure.Arg.NotNull(client);
        Ensure.Arg.NotNull(cache);
        Ensure.Arg.NotNull(clock);
        Ensure.Arg.NotNull(logger);

        _client = client;
        _cache = cache;
        _clock = clock;
        _logger = logger;
    }

    private async Task<OAuthAccessToken> InvokeSynchronized(AuthenticationScheme scheme, Func<Task<OAuthAccessToken>> tokenFunc)
    {
        try
        {
            return await _sync.GetOrAdd(
                                  scheme.Name,
                                  _ => new Lazy<Task<OAuthAccessToken>>(tokenFunc))
                              .Value;
        }
        finally
        {
            _sync.TryRemove(scheme.Name, out _);
        }
    }

    /// <inheritdoc />
    public async Task<OAuthAccessToken> GetClientAccessTokenAsync(
        AuthenticationScheme scheme,
        OAuthAuthenticationParameters? parameters = null,
        CancellationToken cancellationToken = default)
    {
        Ensure.Arg.NotNull(scheme);

        scheme.EnsureClientScheme();

        if (parameters == null || parameters.ForceRenewal == false)
        {
            OAuthAccessToken? item = await _cache.GetAsync(scheme, parameters, cancellationToken);
            if (item != null)
            {
                return item;
            }
        }

        return await InvokeSynchronized(
            scheme,
            async () =>
            {
                TokenResponse response = await _client.RequestClientAccessToken(scheme, parameters, cancellationToken);
                if (response.IsError)
                {
                    _logger.LogError(
                        "Error requesting access token for client scheme {schemeName}. Error = {error}. Error description = {errorDescription}",
                        scheme.Name,
                        response.Error,
                        response.ErrorDescription);

                    throw new AuthenticationException(
                        $"Error requesting access token for client scheme {scheme.Name}");
                }

                OAuthAccessToken token = new(
                    response.AccessToken,
                    response.ExpiresIn > 0
                        ? _clock.UtcNow + TimeSpan.FromSeconds(response.ExpiresIn)
                        : null);

                await _cache.SetAsync(scheme, token, parameters, cancellationToken);
                return token;
            });
    }

    /// <inheritdoc />
    public async Task<OAuthAccessToken> GetPasswordAccessTokenAsync(
        AuthenticationScheme scheme,
        OAuthAuthenticationParameters? parameters = null,
        CancellationToken cancellationToken = default)
    {
        Ensure.Arg.NotNull(scheme);

        scheme.EnsurePasswordScheme();

        if (parameters == null || parameters.ForceRenewal == false)
        {
            OAuthAccessToken? item = await _cache.GetAsync(scheme, parameters, cancellationToken);
            if (item != null)
            {
                return item;
            }
        }

        return await InvokeSynchronized(
            scheme,
            async () =>
            {
                TokenResponse response = await _client.RequestPasswordAccessToken(scheme, parameters, cancellationToken);
                if (response.IsError)
                {
                    _logger.LogError(
                        "Error requesting access token for password scheme {schemeName}. Error = {error}. Error description = {errorDescription}",
                        scheme.Name,
                        response.Error,
                        response.ErrorDescription);

                    throw new AuthenticationException(
                        $"Error requesting access token for password scheme {scheme.Name}");
                }

                OAuthAccessToken token = new(
                    response.AccessToken,
                    response.ExpiresIn > 0
                        ? _clock.UtcNow + TimeSpan.FromSeconds(response.ExpiresIn)
                        : null);

                await _cache.SetAsync(scheme, token, parameters, cancellationToken);
                return token;
            });
    }

    /// <inheritdoc />
    public async Task DeleteAccessTokenAsync(
        AuthenticationScheme scheme,
        OAuthAuthenticationParameters? parameters = null,
        CancellationToken cancellationToken = default)
    {
        Ensure.Arg.NotNull(scheme);
        await _cache.DeleteAsync(scheme, parameters, cancellationToken);
    }
}