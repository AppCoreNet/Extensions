// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using AppCore.Extensions.Http.Authentication.OAuth.AspNetCore;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace AppCore.Extensions.Http.Authentication.OAuth.OpenIdConnect;

/// <summary>
/// Provides the options how to derive <see cref="OAuthClientOptions"/> from <see cref="OpenIdConnectOptions"/>.
/// </summary>
public class OpenIdConnectClientOptions : AuthenticationSchemeOAuthClientOptions
{
    /// <summary>
    /// Scope values as space separated list to use when client configuration is inferred from OpenID Connect scheme.
    /// If not set, token request will omit scope parameter.
    /// </summary>
    public string? Scope { get; set; }

    /// <summary>
    /// Resource value when client configuration is inferred from OpenID Connect scheme.
    /// If not set, token request will omit resource parameter.
    /// </summary>
    public string? Resource { get; set; }
}