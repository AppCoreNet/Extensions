// Licensed under the MIT License.
// Copyright (c) 2018-2022 the AppCore .NET project.

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace AppCore.Extensions.Http.Authentication.OAuth.AspNetCore.OpenIdConnect;

internal sealed class OpenIdConnectOAuthClientOptionsResolver
    : AuthenticationSchemeOAuthClientOptionsResolver<
        OpenIdConnectOAuthClientOptions,
        OpenIdConnectOptions,
        OpenIdConnectHandler
    >
{
    public OpenIdConnectOAuthClientOptionsResolver(
        Microsoft.AspNetCore.Authentication.IAuthenticationSchemeProvider authenticationSchemeProvider,
        IOptionsMonitor<OpenIdConnectOAuthClientOptions> clientOptions,
        IOptionsMonitor<OpenIdConnectOptions> authenticationSchemeOptions)
        : base(authenticationSchemeProvider, authenticationSchemeOptions, clientOptions)
    {
    }

    protected override async Task<OAuthClientOptions> GetOptionsFromSchemeAsync(
        OpenIdConnectOAuthClientOptions clientOptions,
        OpenIdConnectOptions oidcOptions)
    {
        OpenIdConnectConfiguration oidcConfig;
        try
        {
            oidcConfig = await oidcOptions.ConfigurationManager!.GetConfigurationAsync(default);
        }
        catch (Exception e)
        {
            throw new InvalidOperationException(
                $"Unable to load OpenID configuration for configured scheme: {e.Message}");
        }

        var result = new OAuthClientOptions
        {
            TokenEndpoint = new Uri(oidcConfig.TokenEndpoint),
            ClientId = oidcOptions.ClientId,
            ClientSecret = oidcOptions.ClientSecret
        };

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