// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using AppCore.Extensions.Http.Authentication.OAuth.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AppCore.Extensions.Http.Authentication.OAuth.OpenIdConnect;

/// <summary>
/// Represents the OpenID Connect token service.
/// </summary>
public class OpenIdConnectUserTokenService : OAuthUserTokenService<OpenIdConnectUserOptions>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OpenIdConnectUserTokenService"/>.
    /// </summary>
    /// <param name="client"></param>
    /// <param name="store"></param>
    /// <param name="clock"></param>
    /// <param name="optionsMonitor"></param>
    /// <param name="logger"></param>
    public OpenIdConnectUserTokenService(
        IOAuthTokenClient client,
        OpenIdConnectUserTokenStore store,
        ISystemClock clock,
        IOptionsMonitor<OpenIdConnectUserOptions> optionsMonitor,
        ILogger<OpenIdConnectUserTokenService> logger
    )
        : base(client, store, clock, optionsMonitor, logger)
    {
    }

    /// <inheritdoc />
    protected override void EnsureCompatibleScheme(AuthenticationScheme scheme)
    {
        scheme.EnsureOpenIdConnectScheme();
    }
}