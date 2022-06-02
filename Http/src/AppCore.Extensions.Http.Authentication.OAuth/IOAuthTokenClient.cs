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
}