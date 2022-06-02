// Licensed under the MIT License.
// Copyright (c) 2018-2022 the AppCore .NET project.

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace AppCore.Extensions.Http.Authentication.OAuth.AspNetCore;

internal sealed class OpenIdConnectOAuthClientOptionsResolver : IOAuthAuthenticationOptionsResolver
{
    private readonly Microsoft.AspNetCore.Authentication.IAuthenticationSchemeProvider _authenticationSchemeProvider;
    private readonly IOptionsMonitor<OpenIdConnectOAuthClientOptions> _inferredClientOptions;
    private readonly IOptionsMonitor<OpenIdConnectOptions> _openIdConnectionOptions;

    public OpenIdConnectOAuthClientOptionsResolver(
        Microsoft.AspNetCore.Authentication.IAuthenticationSchemeProvider authenticationSchemeProvider,
        IOptionsMonitor<OpenIdConnectOAuthClientOptions> inferredClientOptions,
        IOptionsMonitor<OpenIdConnectOptions> openIdConnectionOptions
    )
    {
        _authenticationSchemeProvider = authenticationSchemeProvider;
        _inferredClientOptions = inferredClientOptions;
        _openIdConnectionOptions = openIdConnectionOptions;
    }

    /// <inheritdoc />
    public async Task<T?> TryGetOptionsAsync<T>(AuthenticationScheme scheme)
        where T : AuthenticationSchemeOptions
    {
        T? result = null;

        if (scheme.OptionsType == typeof(OpenIdConnectOAuthClientOptions)
            && typeof(T) == typeof(OAuthClientAuthenticationOptions))
        {
            result = (T)(object)await GetOptionsFromOpenIdConnectScheme(_inferredClientOptions.Get(scheme.Name));
        }

        return result;
    }

    private async Task<(OpenIdConnectOptions options, OpenIdConnectConfiguration configuration)>
        GetOpenIdConnectSettings(string? schemeName)
    {
        Microsoft.AspNetCore.Authentication.AuthenticationScheme? scheme = string.IsNullOrWhiteSpace(schemeName)
            ? await _authenticationSchemeProvider.GetDefaultChallengeSchemeAsync()
            : await _authenticationSchemeProvider.GetSchemeAsync(schemeName);

        if (scheme is null || scheme.HandlerType != typeof(OpenIdConnectHandler))
        {
            throw new InvalidOperationException(
                "No OpenID Connect authentication scheme configured for getting client configuration. Either set the scheme name explicitly or set the default challenge scheme");
        }

        OpenIdConnectOptions options = _openIdConnectionOptions.Get(scheme.Name);

        OpenIdConnectConfiguration configuration;
        try
        {
            configuration = await options.ConfigurationManager!.GetConfigurationAsync(default);
        }
        catch (Exception e)
        {
            throw new InvalidOperationException(
                $"Unable to load OpenID configuration for configured scheme: {e.Message}");
        }

        return (options, configuration);
    }

    private async Task<OAuthClientAuthenticationOptions> GetOptionsFromOpenIdConnectScheme(OpenIdConnectOAuthClientOptions options)
    {
        (OpenIdConnectOptions oidcOptions, OpenIdConnectConfiguration oidcConfig) = await GetOpenIdConnectSettings(options.Scheme);

        var result = new OAuthClientAuthenticationOptions
        {
            TokenEndpoint = new Uri(oidcConfig.TokenEndpoint),
            ClientId = oidcOptions.ClientId,
            ClientSecret = oidcOptions.ClientSecret
        };

        if (!string.IsNullOrWhiteSpace(options.Scope))
        {
            result.Scope = options.Scope;
        }

        if (!string.IsNullOrWhiteSpace(options.Resource))
        {
            result.Resource.Add(options.Resource);
        }

        return result;
    }
}