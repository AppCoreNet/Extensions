// Licensed under the MIT License.
// Copyright (c) 2018-2022 the AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using AppCore.Extensions.Http.Authentication.OAuth.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace AppCore.Extensions.Http.Authentication.OAuth.OpenIdConnect;

/// <summary>
/// Represents the OpenID Connect user token store.
/// </summary>
public class OpenIdConnectUserTokenStore : AuthenticationSessionOAuthUserTokenStore<OpenIdConnectUserOptions>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OpenIdConnectUserTokenStore"/>.
    /// </summary>
    /// <param name="httpContextAccessor"></param>
    /// <param name="optionsMonitor"></param>
    /// <param name="logger"></param>
    public OpenIdConnectUserTokenStore(
        IHttpContextAccessor httpContextAccessor,
        IOptionsMonitor<OpenIdConnectUserOptions> optionsMonitor,
        ILogger<OpenIdConnectUserTokenStore> logger
    )
        : base(httpContextAccessor, optionsMonitor, logger)
    {
    }

    /// <inheritdoc />
    protected override void EnsureCompatibleScheme(AuthenticationScheme scheme)
    {
        scheme.EnsureOpenIdConnectScheme();
    }

    /// <inheritdoc />
    protected override void StoreToken(
        ClaimsPrincipal principal,
        AuthenticationProperties properties,
        OAuthUserToken token,
        OpenIdConnectUserOptions options)
    {
        List<AuthenticationToken> tokens = new(3)
        {
            new AuthenticationToken { Name = OpenIdConnectParameterNames.AccessToken, Value = token.AccessToken }
        };

        if (!string.IsNullOrWhiteSpace(token.RefreshToken))
        {
            tokens.Add(
                new AuthenticationToken
                    { Name = OpenIdConnectParameterNames.RefreshToken, Value = token.RefreshToken });
        }

        if (token.Expires.HasValue)
        {
            tokens.Add(
                new AuthenticationToken
                {
                    Name = "expires_at",
                    Value = ((DateTimeOffset)token.Expires).ToString("o", CultureInfo.InvariantCulture)
                });
        }

        properties.StoreTokens(tokens);
    }

    /// <inheritdoc />
    protected override OAuthUserToken GetToken(
        ClaimsPrincipal principal,
        AuthenticationProperties properties,
        OpenIdConnectUserOptions options)
    {
        AuthenticationToken[] tokens = properties.GetTokens()
                                                 .ToArray();

        if (tokens.Length == 0)
            throw new AuthenticationException(
                "No tokens found in authentication properties. SaveTokens must be enabled.");

        string? GetTokenValue(string tokenName)
        {
            return tokens.FirstOrDefault(t => string.Equals(t.Name, tokenName, StringComparison.OrdinalIgnoreCase))
                         ?.Value;
        }

        string? accessToken = GetTokenValue(OpenIdConnectParameterNames.AccessToken);
        string? refreshToken = GetTokenValue(OpenIdConnectParameterNames.RefreshToken);
        string? expiresAt = GetTokenValue("expires_at");

        if (string.IsNullOrWhiteSpace(accessToken))
            throw new AuthenticationException("No access token found in authentication properties.");

        return new OAuthUserToken(
            accessToken,
            refreshToken,
            expiresAt != null
                ? DateTimeOffset.Parse(expiresAt, CultureInfo.InvariantCulture)
                : null);
    }
}