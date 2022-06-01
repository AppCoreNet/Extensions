// Licensed under the MIT License.
// Copyright (c) 2018-2022 the AppCore .NET project.

using System;

namespace AppCore.Extensions.Http.Authentication.OAuth;

/// <summary>
/// Provides options for the <see cref="IOAuthTokenCache"/>.
/// </summary>
public class OAuthTokenCacheOptions
{
    /// <summary>
    /// Used to prefix the cache key
    /// </summary>
    public string CacheKeyPrefix { get; set; } = "AppCore.Http.OAuthToken";

    /// <summary>
    /// Value to subtract from token lifetime for the cache entry lifetime (defaults to 3 seconds)
    /// </summary>
    public TimeSpan CacheLifetimeBuffer { get; set; } = TimeSpan.FromSeconds(3);
}