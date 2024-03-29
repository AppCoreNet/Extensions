﻿// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System.Threading.Tasks;
using AppCoreNet.Extensions.Http.Authentication.OAuth.AspNetCore;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Options;

namespace AppCoreNet.Extensions.Http.Authentication.OAuth.OpenIdConnect;

internal sealed class OpenIdConnectUserOptionsResolver
    : AuthenticationSchemeOAuthClientOptionsResolver<
        OpenIdConnectUserOptions,
        OpenIdConnectOptions,
        OpenIdConnectHandler
    >
{
    public OpenIdConnectUserOptionsResolver(
        Microsoft.AspNetCore.Authentication.IAuthenticationSchemeProvider authenticationSchemeProvider,
        IOptionsMonitor<OpenIdConnectUserOptions> clientOptions,
        IOptionsMonitor<OpenIdConnectOptions> authenticationSchemeOptions)
        : base(authenticationSchemeProvider, authenticationSchemeOptions, clientOptions)
    {
    }

    protected override string? GetSchemeName(OpenIdConnectUserOptions options)
    {
        return options.ChallengeScheme;
    }

    protected override async Task<OAuthClientOptions> GetOptionsFromSchemeAsync(
        OpenIdConnectUserOptions clientOptions,
        OpenIdConnectOptions oidcOptions)
    {
        OAuthClientOptions result = await oidcOptions.GetOAuthClientOptionsAsync(default);
        return result;
    }
}