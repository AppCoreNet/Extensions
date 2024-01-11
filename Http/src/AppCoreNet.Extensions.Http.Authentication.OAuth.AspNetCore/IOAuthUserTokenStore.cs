// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace AppCore.Extensions.Http.Authentication.OAuth.AspNetCore;

/// <summary>
/// Abstraction for the OAuth user token store.
/// </summary>
public interface IOAuthUserTokenStore
{
    /// <summary>
    /// Stores the token for the specified authentication scheme and user.
    /// </summary>
    /// <param name="scheme">The <see cref="AuthenticationScheme"/>.</param>
    /// <param name="user">The <see cref="ClaimsPrincipal"/>.</param>
    /// <param name="token">The <see cref="OAuthUserToken"/>.</param>
    /// <param name="cancellationToken">Optional <see cref="CancellationToken"/>.</param>
    Task StoreTokenAsync(
        AuthenticationScheme scheme,
        ClaimsPrincipal user,
        OAuthUserToken token,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the token for the specified authentication scheme and user.
    /// </summary>
    /// <param name="scheme">The <see cref="AuthenticationScheme"/>.</param>
    /// <param name="user">The <see cref="ClaimsPrincipal"/>.</param>
    /// <param name="cancellationToken">Optional <see cref="CancellationToken"/>.</param>
    Task<OAuthUserToken> GetTokenAsync(
        AuthenticationScheme scheme,
        ClaimsPrincipal user,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Clears the token for the specified authentication scheme and user.
    /// </summary>
    /// <param name="scheme">The <see cref="AuthenticationScheme"/>.</param>
    /// <param name="user">The <see cref="ClaimsPrincipal"/>.</param>
    /// <param name="cancellationToken">Optional <see cref="CancellationToken"/>.</param>
    Task ClearTokenAsync(
        AuthenticationScheme scheme,
        ClaimsPrincipal user,
        CancellationToken cancellationToken = default);
}