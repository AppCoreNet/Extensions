// Licensed under the MIT License.
// Copyright (c) 2018-2022 the AppCore .NET project.

using AppCore.Extensions.Http.Authentication.OAuth.AspNetCore;
using Microsoft.AspNetCore.Http;

namespace AppCore.Extensions.Http.Authentication.OAuth.OpenIdConnect;

/// <summary>
/// Represents the OpenID Connect authentication handler.
/// </summary>
public class OpenIdConnectUserHandler : OAuthUserHandler<OpenIdConnectUserParameters>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OpenIdConnectUserHandler"/>.
    /// </summary>
    /// <param name="httpContextAccessor">The <see cref="IHttpContextAccessor"/>.</param>
    /// <param name="authTokenService">The <see cref="OpenIdConnectUserTokenService"/>.</param>
    public OpenIdConnectUserHandler(
        IHttpContextAccessor httpContextAccessor,
        OpenIdConnectUserTokenService authTokenService)
        : base(httpContextAccessor, authTokenService)
    {
    }

    /// <inheritdoc />
    protected override void EnsureCompatibleScheme(AuthenticationScheme scheme)
    {
        scheme.EnsureOpenIdConnectScheme();
    }
}