// Licensed under the MIT License.
// Copyright (c) 2018-2022 the AppCore .NET project.

namespace AppCore.Extensions.Http.Authentication.OAuth;

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