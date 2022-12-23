// Licensed under the MIT License.
// Copyright (c) 2018-2022 the AppCore .NET project.

using System;

namespace AppCore.Extensions.Http.Authentication.OAuth.AspNetCore;

/// <summary>
/// Provides options for HTTP client OAuth user authentication.
/// </summary>
public abstract class OAuthUserOptions : AuthenticationSchemeOptions
{
    /// <summary>
    /// Value to subtract from access token lifetime when testing for expiration.
    /// </summary>
    public TimeSpan RefreshBeforeExpiration { get; set; } = TimeSpan.FromSeconds(3);

    /// <summary>
    /// Gets or sets a value whether to refresh the access token when it has expired.
    /// </summary>
    public bool AllowTokenRefresh { get; set; } = true;

    /// <summary>
    /// Sets the scheme name of the authentication handler from which the user authentication token is being
    /// fetched. This will fallback to the default sign-in scheme if left empty.
    /// </summary>
    public string? SignInScheme { get; set; }

    /// <summary>
    /// Sets the scheme name of the authentication handler from which the token client configuration is
    /// fetched. This will fallback to the default challenge scheme if left empty.
    /// </summary>
    public string? ChallengeScheme { get; set; }
}