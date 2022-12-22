using Microsoft.AspNetCore.Http;

namespace AppCore.Extensions.Http.Authentication.OAuth.AspNetCore.OpenIdConnect;

public class OpenIdConnectUserHandler : OAuthUserHandler<OpenIdConnectUserParameters>
{
    public OpenIdConnectUserHandler(
        IHttpContextAccessor httpContextAccessor,
        OpenIdConnectUserTokenService authTokenService)
        : base(httpContextAccessor, authTokenService)
    {
    }
}