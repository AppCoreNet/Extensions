// Licensed under the MIT License.
// Copyright (c) 2018-2022 the AppCore .NET project.

using System.Threading;
using System.Threading.Tasks;

namespace AppCore.Extensions.Http.Authentication.OAuth;

/// <summary>
/// Abstraction for caching client access tokens
/// </summary>
public interface IOAuthTokenCache
{
    /// <summary>
    /// Caches a access token.
    /// </summary>
    /// <param name="scheme">The <see cref="AuthenticationScheme"/>.</param>
    /// <param name="accessToken">The <see cref="OAuthAccessToken"/>.</param>
    /// <param name="parameters">The <see cref="OAuthAuthenticationParameters"/>.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns></returns>
    Task SetAsync(
        AuthenticationScheme scheme,
        OAuthAccessToken accessToken,
        OAuthAuthenticationParameters? parameters = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a access token from the cache.
    /// </summary>
    /// <param name="scheme">The <see cref="AuthenticationScheme"/>.</param>
    /// <param name="parameters">The <see cref="OAuthAuthenticationParameters"/>.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns></returns>
    Task<OAuthAccessToken?> GetAsync(
        AuthenticationScheme scheme,
        OAuthAuthenticationParameters? parameters = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a access token from the cache.
    /// </summary>
    /// <param name="scheme">The <see cref="AuthenticationScheme"/>.</param>
    /// <param name="parameters">The <see cref="OAuthAuthenticationParameters"/>.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns></returns>
    Task DeleteAsync(
        AuthenticationScheme scheme,
        OAuthAuthenticationParameters? parameters = null,
        CancellationToken cancellationToken = default);
}