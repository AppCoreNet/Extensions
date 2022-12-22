using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace AppCore.Extensions.Http.Authentication.OAuth.AspNetCore.OpenIdConnect;

public class OpenIdConnectUserTokenStore : AuthenticationSessionOAuthUserTokenStore<OpenIdConnectUserOptions>
{
    public OpenIdConnectUserTokenStore(
        IHttpContextAccessor httpContextAccessor,
        IOptionsMonitor<OpenIdConnectUserOptions> optionsMonitor,
        ILogger<OpenIdConnectUserTokenStore> logger)
        : base(httpContextAccessor, optionsMonitor, logger)
    {
    }

    protected override void EnsureCompatibleScheme(AuthenticationScheme scheme)
    {
        if (!typeof(OpenIdConnectUserHandler).IsAssignableFrom(scheme.HandlerType))
            throw new InvalidOperationException($"The client authentication scheme {scheme.Name} is not registered for the OpenID Connect user handler.");
    }

    protected override void SetUserToken(
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

    protected override OAuthUserToken GetUserToken(
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