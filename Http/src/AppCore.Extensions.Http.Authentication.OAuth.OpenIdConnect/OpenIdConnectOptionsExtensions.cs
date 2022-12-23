﻿// Licensed under the MIT License.
// Copyright (c) 2018-2022 the AppCore .NET project.

using System;
using System.Threading;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace AppCore.Extensions.Http.Authentication.OAuth.OpenIdConnect;

internal static class OpenIdConnectOptionsExtensions
{
    public static async Task<OAuthClientOptions> GetOAuthClientOptionsAsync(
        this OpenIdConnectOptions options,
        CancellationToken cancellationToken = default)
    {
        OpenIdConnectConfiguration oidcConfig;
        try
        {
            oidcConfig = await options.ConfigurationManager!.GetConfigurationAsync(cancellationToken)
                                      .ConfigureAwait(false);
        }
        catch (Exception e)
        {
            throw new InvalidOperationException(
                $"Unable to load OpenID configuration for configured scheme: {e.Message}");
        }

        string? tokenRevocationEndpoint = oidcConfig.AdditionalData.TryGetValue(
            OidcConstants.Discovery.RevocationEndpoint,
            out object? value)
            ? value?.ToString()
            : null;

        var result = new OAuthClientOptions
        {
            TokenEndpoint = new Uri(oidcConfig.TokenEndpoint),
            TokenRevocationEndpoint = tokenRevocationEndpoint != null ? new Uri(tokenRevocationEndpoint) : null,
            ClientId = options.ClientId,
            ClientSecret = options.ClientSecret
        };

        return result;
    }
}