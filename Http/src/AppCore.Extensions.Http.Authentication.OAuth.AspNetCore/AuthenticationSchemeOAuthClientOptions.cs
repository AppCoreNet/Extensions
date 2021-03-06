// Licensed under the MIT License.
// Copyright (c) 2018-2022 the AppCore .NET project.

namespace AppCore.Extensions.Http.Authentication.OAuth.AspNetCore;

/// <summary>
/// Provides the options how to derive <see cref="OAuthClientOptions"/> from authentication scheme options.
/// </summary>
public abstract class AuthenticationSchemeOAuthClientOptions : AuthenticationSchemeOptions
{
    /// <summary>
    /// Sets the scheme name of an authentication handler, if the client configuration should be derived from it.
    /// This will fallback to the default challenge scheme if left empty.
    /// </summary>
    public string? Scheme { get; set; }
}