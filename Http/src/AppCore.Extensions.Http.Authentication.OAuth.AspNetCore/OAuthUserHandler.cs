using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AppCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace AppCore.Extensions.Http.Authentication.OAuth.AspNetCore;

/// <summary>
/// Provides a OAuth user authentication handler.
/// </summary>
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

    /// <inheritdoc />
    public async Task AuthenticateAsync(
        AuthenticationScheme scheme,
        TParameters? parameters,
        HttpRequestMessage request,
        CancellationToken cancellationToken = default)
    {
        ClaimsPrincipal? user = _httpContextAccessor.HttpContext?.User;
        if (user == null)
            throw new AuthenticationException("User is not authenticated.");

        OAuthAccessToken accessToken =
            await _authTokenService.GetAccessTokenAsync(scheme, user, parameters, cancellationToken)
                                   .ConfigureAwait(false);

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.AccessToken);
    }
}