// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

namespace AppCoreNet.Extensions.Http.Authentication.OAuth;

/// <summary>
/// Provides the options for the OAuth password authentication scheme.
/// </summary>
public class OAuthPasswordOptions : OAuthOptions
{
    /// <summary>
    /// Gets or sets the username.
    /// </summary>
    public string? Username { get; set; }

    /// <summary>
    /// Gets or sets the password.
    /// </summary>
    public string? Password { get; set; }
}