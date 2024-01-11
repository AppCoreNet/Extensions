// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;

namespace AppCoreNet.Extensions.Http.Authentication.OAuth;

/// <summary>
/// Provides options for the <see cref="IOAuthTokenCache"/>.
/// </summary>
public class OAuthTokenCacheOptions
{
    /// <summary>
    /// Gets or sets the cache key prefix. Defaults to 'AppCore.Http.OAuthToken'.
    /// </summary>
    public string CacheKeyPrefix { get; set; } = "AppCore.Http.OAuthToken";

    /// <summary>
    /// Gets or sets the value to subtract from token lifetime for the cache entry lifetime (defaults to 3 seconds).
    /// </summary>
    public TimeSpan CacheLifetimeBuffer { get; set; } = TimeSpan.FromSeconds(3);
}