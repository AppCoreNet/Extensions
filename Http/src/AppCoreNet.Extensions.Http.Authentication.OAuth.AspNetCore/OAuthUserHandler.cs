// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AppCoreNet.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace AppCoreNet.Extensions.Http.Authentication.OAuth.AspNetCore;

/// <summary>
/// Provides a base class for a OAuth user authentication handler.
/// </summary>
/// <typeparam name="TParameters">The type of the <see cref="OAuthUserParameters"/>.</typeparam>
public abstract class OAuthUserHandler<TParameters> : IAuthenticationSchemeHandler<TParameters>
    where TParameters : OAuthUserParameters
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IOAuthUserTokenService _authTokenService;

    /// <summary>
    /// Initializes a new instance of the <see cref="OAuthUserHandler{TParameters}"/> class.
    /// </summary>
    /// <param name="httpContextAccessor">The <see cref="IHttpContextAccessor"/>.</param>
    /// <param name="authTokenService">The <see cref="IOAuthUserTokenService"/>.</param>
    protected OAuthUserHandler(IHttpContextAccessor httpContextAccessor, IOAuthUserTokenService authTokenService)
    {
        Ensure.Arg.NotNull(httpContextAccessor);
        Ensure.Arg.NotNull(authTokenService);

        _httpContextAccessor = httpContextAccessor;
        _authTokenService = authTokenService;
    }

    /// <summary>
    /// Ensures that the <paramref name="scheme"/> is compatible.
    /// </summary>
    /// <param name="scheme">The <see cref="AuthenticationScheme"/>.</param>
    protected abstract void EnsureCompatibleScheme(AuthenticationScheme scheme);

    /// <inheritdoc />
    public async Task AuthenticateAsync(
        AuthenticationScheme scheme,
        HttpRequestMessage request,
        TParameters? parameters = null,
        CancellationToken cancellationToken = default)
    {
        Ensure.Arg.NotNull(scheme);
        Ensure.Arg.NotNull(request);

        EnsureCompatibleScheme(scheme);

        ClaimsPrincipal? user = _httpContextAccessor.HttpContext?.User;
        if (user == null)
            throw new AuthenticationException("User is not authenticated.");

        OAuthAccessToken accessToken =
            await _authTokenService.GetAccessTokenAsync(scheme, user, parameters, cancellationToken)
                                   .ConfigureAwait(false);

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.AccessToken);
    }
}