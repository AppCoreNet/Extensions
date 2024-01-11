// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System.Threading;
using System.Threading.Tasks;

namespace AppCoreNet.Extensions.Http.Authentication.OAuth;

/// <summary>
/// Abstraction for caching client access tokens.
/// </summary>
public interface IOAuthTokenCache
{
    /// <summary>
    /// Caches a access token.
    /// </summary>
    /// <param name="scheme">The <see cref="AuthenticationScheme"/>.</param>
    /// <param name="accessToken">The <see cref="OAuthAccessToken"/>.</param>
    /// <param name="parameters">The <see cref="OAuthParameters"/>.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task SetAsync(
        AuthenticationScheme scheme,
        OAuthAccessToken accessToken,
        OAuthParameters? parameters = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a access token from the cache.
    /// </summary>
    /// <param name="scheme">The <see cref="AuthenticationScheme"/>.</param>
    /// <param name="parameters">The <see cref="OAuthParameters"/>.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task<OAuthAccessToken?> GetAsync(
        AuthenticationScheme scheme,
        OAuthParameters? parameters = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a access token from the cache.
    /// </summary>
    /// <param name="scheme">The <see cref="AuthenticationScheme"/>.</param>
    /// <param name="parameters">The <see cref="OAuthParameters"/>.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task DeleteAsync(
        AuthenticationScheme scheme,
        OAuthParameters? parameters = null,
        CancellationToken cancellationToken = default);
}