// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;
using AppCoreNet.Diagnostics;
using IdentityModel.Client;
using Microsoft.Extensions.Logging;

namespace AppCoreNet.Extensions.Http.Authentication.OAuth;

/// <summary>
/// Service for getting OAuth access tokens.
/// </summary>
public class OAuthTokenService : IOAuthTokenService
{
    private static readonly ConcurrentDictionary<string, Lazy<Task<OAuthAccessToken>>> _sync = new ();
    private readonly IOAuthTokenClient _client;
    private readonly IOAuthTokenCache _cache;
    private readonly ILogger<OAuthTokenService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="OAuthTokenService"/> class.
    /// </summary>
    /// <param name="client">The <see cref="IOAuthTokenClient"/>.</param>
    /// <param name="cache">The <see cref="IOAuthTokenCache"/>.</param>
    /// <param name="logger">The <see cref="ILogger"/>.</param>
    public OAuthTokenService(IOAuthTokenClient client, IOAuthTokenCache cache, ILogger<OAuthTokenService> logger)
    {
        Ensure.Arg.NotNull(client);
        Ensure.Arg.NotNull(cache);
        Ensure.Arg.NotNull(logger);

        _client = client;
        _cache = cache;
        _logger = logger;
    }

    [SuppressMessage("Usage", "VSTHRD011:Use AsyncLazy<T>", Justification = "ConfigureAwait(false) is used")]
    private async Task<OAuthAccessToken> InvokeSynchronizedAsync(AuthenticationScheme scheme, Func<Task<OAuthAccessToken>> tokenFunc)
    {
        try
        {
            return await _sync.GetOrAdd(
                                  scheme.Name,
                                  _ => new Lazy<Task<OAuthAccessToken>>(tokenFunc))
                              .Value.ConfigureAwait(false);
        }
        finally
        {
            _sync.TryRemove(scheme.Name, out _);
        }
    }

    /// <inheritdoc />
    public async Task<OAuthAccessToken> GetClientAccessTokenAsync(
        AuthenticationScheme scheme,
        OAuthParameters? parameters = null,
        CancellationToken cancellationToken = default)
    {
        Ensure.Arg.NotNull(scheme);

        scheme.EnsureClientScheme();

        if (parameters == null || parameters.ForceRenewal == false)
        {
            OAuthAccessToken? item =
                await _cache.GetAsync(scheme, parameters, cancellationToken)
                            .ConfigureAwait(false);

            if (item != null)
            {
                return item;
            }
        }

        return await InvokeSynchronizedAsync(
                scheme,
                async () =>
                {
                    _logger.LogDebug("Requesting access token for client scheme {SchemeName} ...", scheme.Name);

                    TokenResponse response =
                        await _client.RequestClientAccessTokenAsync(scheme, parameters, cancellationToken)
                                     .ConfigureAwait(false);

                    if (response.IsError)
                    {
                        _logger.LogError(
                            "Error requesting access token for client scheme {SchemeName}. Error = {Error}. Error description = {ErrorDescription}",
                            scheme.Name,
                            response.Error,
                            response.ErrorDescription);

                        throw new AuthenticationException(
                            $"Error requesting access token for client scheme '{scheme.Name}': {response.Error}");
                    }

                    OAuthAccessToken token = new (
                        response.AccessToken!,
                        response.ExpiresIn > 0 ? DateTimeOffset.UtcNow + TimeSpan.FromSeconds(response.ExpiresIn) : null);

                    _logger.LogDebug(
                        "Received access token for client scheme {SchemeName}. Expiration: {Expiration}",
                        scheme.Name,
                        token.Expires);

                    await _cache.SetAsync(scheme, token, parameters, cancellationToken)
                                .ConfigureAwait(false);

                    return token;
                })
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<OAuthAccessToken> GetPasswordAccessTokenAsync(
        AuthenticationScheme scheme,
        OAuthParameters? parameters = null,
        CancellationToken cancellationToken = default)
    {
        Ensure.Arg.NotNull(scheme);

        scheme.EnsurePasswordScheme();

        if (parameters == null || parameters.ForceRenewal == false)
        {
            OAuthAccessToken? item =
                await _cache.GetAsync(scheme, parameters, cancellationToken)
                            .ConfigureAwait(false);

            if (item != null)
            {
                return item;
            }
        }

        return await InvokeSynchronizedAsync(
                scheme,
                async () =>
                {
                    _logger.LogDebug("Requesting access token for password scheme {SchemeName} ...", scheme.Name);

                    TokenResponse response =
                        await _client.RequestPasswordAccessTokenAsync(scheme, parameters, cancellationToken)
                                     .ConfigureAwait(false);

                    if (response.IsError)
                    {
                        _logger.LogError(
                            "Error requesting access token for password scheme {SchemeName}. Error = {Error}. Error description = {ErrorDescription}",
                            scheme.Name,
                            response.Error,
                            response.ErrorDescription);

                        throw new AuthenticationException(
                            $"Error requesting access token for password scheme '{scheme.Name}'");
                    }

                    OAuthAccessToken token = new (
                        response.AccessToken!,
                        response.ExpiresIn > 0 ? DateTimeOffset.UtcNow + TimeSpan.FromSeconds(response.ExpiresIn) : null);

                    _logger.LogDebug(
                        "Received access token for password scheme {SchemeName}. Expiration: {Expiration}",
                        scheme.Name,
                        token.Expires);

                    await _cache.SetAsync(scheme, token, parameters, cancellationToken)
                                .ConfigureAwait(false);

                    return token;
                })
            .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task DeleteAccessTokenAsync(
        AuthenticationScheme scheme,
        OAuthParameters? parameters = null,
        CancellationToken cancellationToken = default)
    {
        Ensure.Arg.NotNull(scheme);

        await _cache.DeleteAsync(scheme, parameters, cancellationToken)
                    .ConfigureAwait(false);
    }
}