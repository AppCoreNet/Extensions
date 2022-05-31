// Licensed under the MIT License.
// Copyright (c) 2018-2022 the AppCore .NET project.

using System;
using System.Threading;
using System.Threading.Tasks;
using AppCore.Diagnostics;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AppCore.Extensions.Http.Authentication.OAuth;

/// <summary>
/// Provides a cache for OAuth access tokens.
/// </summary>
public class OAuthTokenCache : IOAuthTokenCache
{
    private readonly IMemoryCache _cache;
    private readonly IOptionsMonitor<OAuthTokenCacheOptions> _optionsMonitor;
    private readonly ILogger<OAuthTokenCache> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="OAuthTokenCache"/> class.
    /// </summary>
    /// <param name="cache">The <see cref="IMemoryCache"/>.</param>
    /// <param name="optionsMonitor">The options.</param>
    /// <param name="logger">The <see cref="ILogger{TCategoryName}"/>.</param>
    public OAuthTokenCache(IMemoryCache cache, IOptionsMonitor<OAuthTokenCacheOptions> optionsMonitor, ILogger<OAuthTokenCache> logger)
    {
        _cache = cache;
        _optionsMonitor = optionsMonitor;
        _logger = logger;
    }

    /// <inheritdoc />
    public Task SetAsync(
        AuthenticationScheme scheme,
        OAuthAccessToken accessToken,
        OAuthAuthenticationParameters? parameters = null,
        CancellationToken cancellationToken = default)
    {
        OAuthTokenCacheOptions options = _optionsMonitor.CurrentValue;

        DateTimeOffset expiration = accessToken.Expires.GetValueOrDefault(DateTimeOffset.MaxValue);
        DateTimeOffset cacheExpiration = expiration - options.CacheLifetimeBuffer;

        _logger.LogDebug(
            "Caching access token for scheme: {schemeName}. Expiration: {expiration}",
            scheme.Name,
            cacheExpiration);

        string cacheKey = GenerateCacheKey(scheme, options, parameters);
        _cache.Set(cacheKey, accessToken, cacheExpiration);

        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public Task<OAuthAccessToken?> GetAsync(
        AuthenticationScheme scheme,
        OAuthAuthenticationParameters? parameters = null,
        CancellationToken cancellationToken = default)
    {
        Ensure.Arg.NotNull(scheme, nameof(scheme));

        OAuthTokenCacheOptions options = _optionsMonitor.CurrentValue;

        string cacheKey = GenerateCacheKey(scheme, options, parameters);
        var result = _cache.Get<OAuthAccessToken?>(cacheKey);

        if (result != null)
        {
            _logger.LogDebug("Cache hit for access token for scheme: {schemeName}", scheme.Name);
        }
        else
        {
            _logger.LogDebug("Cache miss for access token for scheme: {schemeName}", scheme.Name);
        }

        return Task.FromResult(result);
    }

    /// <inheritdoc />
    public Task DeleteAsync(
        AuthenticationScheme scheme,
        OAuthAuthenticationParameters? parameters = null,
        CancellationToken cancellationToken = default)
    {
        Ensure.Arg.NotNull(scheme, nameof(scheme));

        OAuthTokenCacheOptions options = _optionsMonitor.CurrentValue;

        string cacheKey = GenerateCacheKey(scheme, options, parameters);
        _cache.Remove(cacheKey);

        return Task.CompletedTask;
    }

    /// <summary>
    /// Generates the cache key based on various inputs.
    /// </summary>
    /// <param name="scheme">The <see cref="AuthenticationScheme"/>.</param>
    /// <param name="options">The <see cref="OAuthTokenCacheOptions"/>.</param>
    /// <param name="parameters">The <see cref="OAuthAuthenticationParameters"/>.</param>
    /// <returns></returns>
    protected virtual string GenerateCacheKey(
        AuthenticationScheme scheme,
        OAuthTokenCacheOptions options,
        OAuthAuthenticationParameters? parameters = null)
    {
        return options.CacheKeyPrefix + "::" + scheme.Name + "::" + parameters?.Resource ?? "";
    }
}