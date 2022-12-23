// Licensed under the MIT License.
// Copyright (c) 2018-2022 the AppCore .NET project.

using System;

namespace AppCore.Extensions.Http.Authentication.OAuth.AspNetCore;

/// <summary>
/// Represents a user OAuth token.
/// </summary>
public class OAuthUserToken : OAuthAccessToken
{
    /// <summary>
    /// Gets the refresh token.
    /// </summary>
    public string? RefreshToken { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="OAuthUserToken"/> class.
    /// </summary>
    /// <param name="accessToken">The access token.</param>
    /// <param name="refreshToken">The refresh token.</param>
    /// <param name="expires">A value indicating when the access token will expire; <c>null</c> if the token will never expire.</param>
    public OAuthUserToken(string accessToken, string? refreshToken, DateTimeOffset? expires)
        : base(accessToken, expires)
    {
        RefreshToken = refreshToken;
    }
}