// Licensed under the MIT License.
// Copyright (c) 2018-2022 the AppCore .NET project.

using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Options;

namespace AppCore.Extensions.Http.Authentication.OAuth.AspNetCore.OpenIdConnect;

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