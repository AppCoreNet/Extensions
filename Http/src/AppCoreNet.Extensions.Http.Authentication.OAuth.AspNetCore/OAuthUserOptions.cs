// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;

namespace AppCoreNet.Extensions.Http.Authentication.OAuth.AspNetCore;

/// <summary>
/// Provides options for HTTP client OAuth user authentication.
/// </summary>
public abstract class OAuthUserOptions : AuthenticationSchemeOptions
{
    /// <summary>
    /// Gets or sets a value to subtract from access token lifetime when testing for expiration.
    /// </summary>
    public TimeSpan RefreshBeforeExpiration { get; set; } = TimeSpan.FromSeconds(3);

    /// <summary>
    /// Gets or sets a value indicating whether to refresh the access token when it has expired.
    /// </summary>
    public bool AllowTokenRefresh { get; set; } = true;

    /// <summary>
    /// Gets or sets the scheme name of the authentication handler from which the user authentication token is being
    /// fetched. This will fallback to the default sign-in scheme if left empty.
    /// </summary>
    public string? SignInScheme { get; set; }

    /// <summary>
    /// Gets or sets the scheme name of the authentication handler from which the token client configuration is
    /// fetched. This will fallback to the default challenge scheme if left empty.
    /// </summary>
    public string? ChallengeScheme { get; set; }
}