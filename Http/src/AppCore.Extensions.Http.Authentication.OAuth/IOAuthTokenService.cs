// Licensed under the MIT License.
// Copyright (c) 2018-2022 the AppCore .NET project.

using System.Threading;
using System.Threading.Tasks;

namespace AppCore.Extensions.Http.Authentication.OAuth;

/// <summary>
/// Abstraction for the OAuth token service.
/// </summary>
public interface IOAuthTokenService
{
    /// <summary>
    /// Returns either a cached or a new access token for a given client configuration or the default client
    /// </summary>
    /// <param name="scheme">The <see cref="AuthenticationScheme"/>.</param>
    /// <param name="parameters">The <see cref="OAuthAuthenticationParameters"/>.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>The access token or null if the no token can be requested.</returns>
    Task<OAuthAccessToken> GetClientAccessTokenAsync(
        AuthenticationScheme scheme,
        OAuthAuthenticationParameters? parameters = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns either a cached or a new access token for a given password configuration or the default client
    /// </summary>
    /// <param name="scheme">The <see cref="AuthenticationScheme"/>.</param>
    /// <param name="parameters">The <see cref="OAuthAuthenticationParameters"/>.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>The access token or null if the no token can be requested.</returns>
    Task<OAuthAccessToken> GetPasswordAccessTokenAsync(
        AuthenticationScheme scheme,
        OAuthAuthenticationParameters? parameters = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a access token from the cache
    /// </summary>
    /// <param name="scheme">The <see cref="AuthenticationScheme"/>.</param>
    /// <param name="parameters">The <see cref="OAuthAuthenticationParameters"/>.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns></returns>
    Task DeleteAccessTokenAsync(
        AuthenticationScheme scheme,
        OAuthAuthenticationParameters? parameters = null,
        CancellationToken cancellationToken = default);
}