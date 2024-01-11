// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System.Threading;
using System.Threading.Tasks;

namespace AppCoreNet.Extensions.Http.Authentication.OAuth;

/// <summary>
/// Abstraction for the OAuth token service.
/// </summary>
public interface IOAuthTokenService
{
    /// <summary>
    /// Returns either a cached or a new access token for a given client configuration or the default client.
    /// </summary>
    /// <param name="scheme">The <see cref="AuthenticationScheme"/>.</param>
    /// <param name="parameters">The <see cref="OAuthParameters"/>.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>The access token or null if the no token can be requested.</returns>
    Task<OAuthAccessToken> GetClientAccessTokenAsync(
        AuthenticationScheme scheme,
        OAuthParameters? parameters = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns either a cached or a new access token for a given password configuration or the default client.
    /// </summary>
    /// <param name="scheme">The <see cref="AuthenticationScheme"/>.</param>
    /// <param name="parameters">The <see cref="OAuthParameters"/>.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>The access token or null if the no token can be requested.</returns>
    Task<OAuthAccessToken> GetPasswordAccessTokenAsync(
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
    Task DeleteAccessTokenAsync(
        AuthenticationScheme scheme,
        OAuthParameters? parameters = null,
        CancellationToken cancellationToken = default);
}