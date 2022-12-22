using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace AppCore.Extensions.Http.Authentication.OAuth.AspNetCore;

public interface IOAuthUserTokenStore
{
    Task StoreTokenAsync(
        AuthenticationScheme scheme,
        ClaimsPrincipal user,
        OAuthUserToken token,
        OAuthUserParameters? parameters = null,
        CancellationToken cancellationToken = default);

    Task<OAuthUserToken> GetTokenAsync(
        AuthenticationScheme scheme,
        ClaimsPrincipal user,
        OAuthUserParameters? parameters = null,
        CancellationToken cancellationToken = default);

    Task ClearTokenAsync(
        AuthenticationScheme scheme,
        ClaimsPrincipal user,
        OAuthUserParameters? parameters = null,
        CancellationToken cancellationToken = default);
}