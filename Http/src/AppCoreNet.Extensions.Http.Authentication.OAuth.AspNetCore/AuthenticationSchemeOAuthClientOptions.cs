// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

namespace AppCoreNet.Extensions.Http.Authentication.OAuth.AspNetCore;

/// <summary>
/// Provides the options how to derive <see cref="OAuthClientOptions"/> from authentication scheme options.
/// </summary>
public abstract class AuthenticationSchemeOAuthClientOptions : AuthenticationSchemeOptions
{
    /// <summary>
    /// Gets or sets the scheme name of an authentication handler, if the client configuration should be derived from it.
    /// This will fallback to the default challenge scheme if left empty.
    /// </summary>
    public string? Scheme { get; set; }
}