// Licensed under the MIT License.
// Copyright (c) 2018-2022 the AppCore .NET project.

using System.Threading;
using System.Threading.Tasks;
using IdentityModel.Client;

namespace AppCore.Extensions.Http.Authentication.OAuth;

/// <summary>
/// Represents a OAuth token client.
/// </summary>
public interface IOAuthTokenClient
{
    /// <summary>
    /// Requests a client access token.
    /// </summary>
    /// <param name="scheme">The <see cref="AuthenticationScheme"/>.</param>
    /// <param name="parameters">The <see cref="OAuthParameters"/>.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns></returns>
    Task<TokenResponse> RequestClientAccessToken(
        AuthenticationScheme scheme,
        OAuthParameters? parameters = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Requests a password access token.
    /// </summary>
    /// <param name="scheme">The <see cref="AuthenticationScheme"/>.</param>
    /// <param name="parameters">The <see cref="OAuthParameters"/>.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns></returns>
    Task<TokenResponse> RequestPasswordAccessToken(
        AuthenticationScheme scheme,
        OAuthParameters? parameters = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Requests a token refresh.
    /// </summary>
    /// <param name="scheme">The <see cref="AuthenticationScheme"/>.</param>
    /// <param name="refreshToken">The refresh token.</param>
    /// <param name="parameters">The <see cref="OAuthParameters"/>.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns></returns>
    Task<TokenResponse> RequestRefreshTokenAsync(
        AuthenticationScheme scheme,
        string refreshToken,
        OAuthParameters? parameters = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Revokes a token.
    /// </summary>
    /// <param name="scheme">The <see cref="AuthenticationScheme"/>.</param>
    /// <param name="token">The token to revoke.</param>
    /// <param name="tokenTypeHint">A hint for the type of the token.</param>
    /// <param name="parameters">The <see cref="OAuthParameters"/>.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns></returns>
    Task<TokenRevocationResponse> RevokeTokenAsync(
        AuthenticationScheme scheme,
        string token,
        string tokenTypeHint,
        OAuthParameters? parameters = null,
        CancellationToken cancellationToken = default);
}