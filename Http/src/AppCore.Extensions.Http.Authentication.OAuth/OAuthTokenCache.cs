// Licensed under the MIT License.
// Copyright (c) 2018-2022 the AppCore .NET project.

using System;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AppCore.Diagnostics;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AppCore.Extensions.Http.Authentication.OAuth;

/// <summary>
/// Provides a cache for OAuth access tokens.
/// </summary>
public class OAuthTokenCache : IOAuthTokenCache
{
    private readonly IDistributedCache _cache;
    private readonly IOptionsMonitor<OAuthTokenCacheOptions> _optionsMonitor;
    private readonly ILogger<OAuthTokenCache> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="OAuthTokenCache"/> class.
    /// </summary>
    /// <param name="cache">The <see cref="IMemoryCache"/>.</param>
    /// <param name="optionsMonitor">The options.</param>
    /// <param name="logger">The <see cref="ILogger{TCategoryName}"/>.</param>
    public OAuthTokenCache(IDistributedCache cache, IOptionsMonitor<OAuthTokenCacheOptions> optionsMonitor, ILogger<OAuthTokenCache> logger)
    {
        _cache = cache;
        _optionsMonitor = optionsMonitor;
        _logger = logger;
    }

    private static byte[] SerializeAccessToken(OAuthAccessToken token)
    {
        var builder = new StringBuilder();
        builder.Append(token.AccessToken);
        if (token.Expires.HasValue)
        {
            builder.Append(':');
            builder.Append(
                token.Expires.Value.ToUnixTimeSeconds()
                     .ToString("D"));
        }

        return Encoding.UTF8.GetBytes(builder.ToString());
    }

    private static OAuthAccessToken? DeserializeAccessToken(byte[]? bytes)
    {
        if (bytes == null)
            return null;

        string[] values = Encoding.UTF8.GetString(bytes).Split(new [] { ':' });

        if (values.Length == 0)
            return null;

        string accessToken = values[0];
        DateTimeOffset? expires = null;

        if (values.Length > 1)
        {
            if (!long.TryParse(values[1], out long expiresSeconds))
                return null;

            expires = DateTimeOffset.FromUnixTimeSeconds(expiresSeconds);
        }

        return new OAuthAccessToken(accessToken, expires);
    }

    /// <inheritdoc />
    public async Task SetAsync(
        AuthenticationScheme scheme,
        OAuthAccessToken accessToken,
        OAuthParameters? parameters = null,
        CancellationToken cancellationToken = default)
    {
        OAuthTokenCacheOptions options = _optionsMonitor.CurrentValue;

        DateTimeOffset expiration = accessToken.Expires.GetValueOrDefault(DateTimeOffset.MaxValue);
        DateTimeOffset cacheExpiration = expiration - options.CacheLifetimeBuffer;

        _logger.LogDebug(
            "Caching access token for scheme: {schemeName}. Expiration: {expiration}",
            scheme.Name,
            cacheExpiration);

        await _cache.SetAsync(
                        GenerateCacheKey(scheme, options, parameters),
                        SerializeAccessToken(accessToken),
                        new DistributedCacheEntryOptions { AbsoluteExpiration = cacheExpiration },
                        cancellationToken)
                    .ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<OAuthAccessToken?> GetAsync(
        AuthenticationScheme scheme,
        OAuthParameters? parameters = null,
        CancellationToken cancellationToken = default)
    {
        Ensure.Arg.NotNull(scheme);

        OAuthTokenCacheOptions options = _optionsMonitor.CurrentValue;

        string cacheKey = GenerateCacheKey(scheme, options, parameters);
        OAuthAccessToken? result = DeserializeAccessToken(
            await _cache.GetAsync(cacheKey, cancellationToken)
                        .ConfigureAwait(false));

        if (result != null)
        {
            _logger.LogTrace("Cache hit for access token for scheme: {schemeName}", scheme.Name);
        }
        else
        {
            _logger.LogTrace("Cache miss for access token for scheme: {schemeName}", scheme.Name);
        }

        return result;
    }

    /// <inheritdoc />
    public Task DeleteAsync(
        AuthenticationScheme scheme,
        OAuthParameters? parameters = null,
        CancellationToken cancellationToken = default)
    {
        Ensure.Arg.NotNull(scheme);

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
    /// <param name="parameters">The <see cref="OAuthParameters"/>.</param>
    /// <returns></returns>
    protected virtual string GenerateCacheKey(
        AuthenticationScheme scheme,
        OAuthTokenCacheOptions options,
        OAuthParameters? parameters = null)
    {
        return options.CacheKeyPrefix + "::" + scheme.Name + "::" + parameters?.Resource ?? "";
    }
}