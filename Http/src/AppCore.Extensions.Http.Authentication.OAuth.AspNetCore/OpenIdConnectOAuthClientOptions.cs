// Licensed under the MIT License.
// Copyright (c) 2018-2022 the AppCore .NET project.

namespace AppCore.Extensions.Http.Authentication.OAuth.AspNetCore;

public class OpenIdConnectOAuthClientOptions
{
    /// <summary>
    /// Sets the scheme name of an OpenID Connect handler, if the client configuration should be derived from it.
    /// This will be used as a default if no explicit clients are configured (and will fallback to the default challenge scheme if left empty).
    /// </summary>
    public string? Scheme { get; set; }

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