// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using AppCoreNet.Diagnostics;

namespace AppCore.Extensions.Http.Authentication.OAuth;

/// <summary>
/// Represents a OAuth access token.
/// </summary>
public class OAuthAccessToken
{
    /// <summary>
    /// Gets the access token.
    /// </summary>
    public string AccessToken { get; }

    /// <summary>
    /// Gets a value indicating when the access token will expire.
    /// </summary>
    public DateTimeOffset? Expires { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="OAuthAccessToken"/> class.
    /// </summary>
    /// <param name="accessToken">The access token.</param>
    /// <param name="expires">A value indicating when the access token will expire; <c>null</c> if the token will never expire.</param>
    public OAuthAccessToken(string accessToken, DateTimeOffset? expires)
    {
        Ensure.Arg.NotEmpty(accessToken);

        AccessToken = accessToken;
        Expires = expires;
    }
}