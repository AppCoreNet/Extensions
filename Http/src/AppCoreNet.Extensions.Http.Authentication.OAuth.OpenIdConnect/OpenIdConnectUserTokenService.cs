// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using AppCoreNet.Extensions.Http.Authentication.OAuth.AspNetCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AppCoreNet.Extensions.Http.Authentication.OAuth.OpenIdConnect;

/// <summary>
/// Represents the OpenID Connect token service.
/// </summary>
public class OpenIdConnectUserTokenService : OAuthUserTokenService<OpenIdConnectUserOptions>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OpenIdConnectUserTokenService"/> class.
    /// </summary>
    /// <param name="client">The <see cref="IOAuthTokenClient"/>.</param>
    /// <param name="store">The <see cref="OpenIdConnectUserTokenStore"/>.</param>
    /// <param name="timeProvider">The <see cref="TimeProvider"/>.</param>
    /// <param name="optionsMonitor">The <see cref="IOptionsMonitor{TOptions}"/>.</param>
    /// <param name="logger">The <see cref="ILogger"/>.</param>
    public OpenIdConnectUserTokenService(
        IOAuthTokenClient client,
        OpenIdConnectUserTokenStore store,
        TimeProvider timeProvider,
        IOptionsMonitor<OpenIdConnectUserOptions> optionsMonitor,
        ILogger<OpenIdConnectUserTokenService> logger)
        : base(client, store, timeProvider, optionsMonitor, logger)
    {
    }

    /// <inheritdoc />
    protected override void EnsureCompatibleScheme(AuthenticationScheme scheme)
    {
        scheme.EnsureOpenIdConnectScheme();
    }
}