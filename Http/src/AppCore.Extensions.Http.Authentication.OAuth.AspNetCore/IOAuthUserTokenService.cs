// Licensed under the MIT License.
// Copyright (c) 2018-2022 the AppCore .NET project.

using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace AppCore.Extensions.Http.Authentication.OAuth.AspNetCore;

/// <summary>
/// Abstraction for the OAuth user token service.
/// </summary>
public interface IOAuthUserTokenService
{
    /// <summary>
    /// Gets the access token for the specified scheme and user. If the access token is expired
    /// it will be automatically refreshed, if permitted.
    /// </summary>
    /// <param name="scheme">The <see cref="AuthenticationScheme"/>.</param>
    /// <param name="user">The user.</param>
    /// <param name="parameters">Optional parameters.</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<OAuthUserToken> GetAccessTokenAsync(
        AuthenticationScheme scheme,
        ClaimsPrincipal user,
        OAuthUserParameters? parameters = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Revokes the refresh token for the specified scheme and user.
    /// </summary>
    /// <param name="scheme">The <see cref="AuthenticationScheme"/>.</param>
    /// <param name="user">The user.</param>
    /// <param name="parameters">Optional parameters.</param>
    /// <param name="cancellationToken"></param>
    Task RevokeRefreshTokenAsync(
        AuthenticationScheme scheme,
        ClaimsPrincipal user,
        OAuthUserParameters? parameters = null,
        CancellationToken cancellationToken = default);
}