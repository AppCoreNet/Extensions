using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using AppCore.Diagnostics;

namespace AppCore.Extensions.Http.Authentication.OAuth;

/// <summary>
/// Provides a OAuth password authentication handler.
/// </summary>
public class OAuthPasswordAuthenticationHandler : IAuthenticationSchemeHandler<OAuthAuthenticationParameters>
{
    private readonly IOAuthTokenService _authTokenService;

    /// <summary>
    /// Initializes a new instance of the <see cref="OAuthPasswordAuthenticationHandler"/> class.
    /// </summary>
    /// <param name="authTokenService">The <see cref="IOAuthTokenService"/>.</param>
    public OAuthPasswordAuthenticationHandler(IOAuthTokenService authTokenService)
    {
        Ensure.Arg.NotNull(authTokenService);
        _authTokenService = authTokenService;
    }

    /// <inheritdoc />
    public async Task AuthenticateAsync(
        AuthenticationScheme scheme,
        OAuthAuthenticationParameters? parameters,
        HttpRequestMessage request,
        CancellationToken cancellationToken = default)
    {
        OAuthAccessToken accessToken =
            await _authTokenService.GetPasswordAccessTokenAsync(scheme, parameters, cancellationToken);

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.AccessToken);
    }
}