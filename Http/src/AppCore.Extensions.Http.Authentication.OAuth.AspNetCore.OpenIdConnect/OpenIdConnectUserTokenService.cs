using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AppCore.Extensions.Http.Authentication.OAuth.AspNetCore.OpenIdConnect;

public class OpenIdConnectUserTokenService : OAuthUserTokenService<OpenIdConnectUserOptions>
{
    public OpenIdConnectUserTokenService(
        IOAuthTokenClient client,
        OpenIdConnectUserTokenStore store,
        ISystemClock clock,
        IOptionsMonitor<OpenIdConnectUserOptions> optionsMonitor,
        ILogger<OpenIdConnectUserTokenService> logger)
        : base(client, store, clock, optionsMonitor, logger)
    {
    }

    protected override void EnsureCompatibleScheme(AuthenticationScheme scheme)
    {
        if (!typeof(OpenIdConnectUserHandler).IsAssignableFrom(scheme.HandlerType))
            throw new InvalidOperationException($"The client authentication scheme {scheme.Name} is not registered for the OpenID Connect user handler.");
    }
}