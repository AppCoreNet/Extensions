﻿// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using AppCoreNet.Extensions.Http.Authentication.OAuth.AspNetCore;
using Microsoft.AspNetCore.Http;

namespace AppCoreNet.Extensions.Http.Authentication.OAuth.OpenIdConnect;

/// <summary>
/// Represents the OpenID Connect authentication handler.
/// </summary>
public class OpenIdConnectUserHandler : OAuthUserHandler<OpenIdConnectUserParameters>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OpenIdConnectUserHandler"/> class.
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