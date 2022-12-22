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

    public OAuthUserToken(string accessToken, string? refreshToken, DateTimeOffset? expires)
        : base(accessToken, expires)
    {
        RefreshToken = refreshToken;
    }
}