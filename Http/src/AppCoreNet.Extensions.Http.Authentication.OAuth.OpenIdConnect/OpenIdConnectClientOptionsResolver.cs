// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System.Threading.Tasks;
using AppCoreNet.Extensions.Http.Authentication.OAuth.AspNetCore;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Options;

namespace AppCoreNet.Extensions.Http.Authentication.OAuth.OpenIdConnect;

internal sealed class OpenIdConnectClientOptionsResolver
    : AuthenticationSchemeOAuthClientOptionsResolver<
        OpenIdConnectClientOptions,
        OpenIdConnectOptions,
        OpenIdConnectHandler
    >
{
    public OpenIdConnectClientOptionsResolver(
        Microsoft.AspNetCore.Authentication.IAuthenticationSchemeProvider authenticationSchemeProvider,
        IOptionsMonitor<OpenIdConnectClientOptions> clientOptions,
        IOptionsMonitor<OpenIdConnectOptions> authenticationSchemeOptions)
        : base(authenticationSchemeProvider, authenticationSchemeOptions, clientOptions)
    {
    }

    protected override string? GetSchemeName(OpenIdConnectClientOptions options)
    {
        return options.Scheme;
    }

    protected override async Task<OAuthClientOptions> GetOptionsFromSchemeAsync(
        OpenIdConnectClientOptions clientOptions,
        OpenIdConnectOptions oidcOptions)
    {
        OAuthClientOptions result = await oidcOptions.GetOAuthClientOptionsAsync(default);

        if (!string.IsNullOrWhiteSpace(clientOptions.Scope))
        {
            result.Scope = clientOptions.Scope;
        }

        if (!string.IsNullOrWhiteSpace(clientOptions.Resource))
        {
            result.Resource.Add(clientOptions.Resource);
        }

        return result;
    }
}