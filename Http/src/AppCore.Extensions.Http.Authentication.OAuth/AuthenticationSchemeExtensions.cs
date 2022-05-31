// Licensed under the MIT License.
// Copyright (c) 2018-2022 the AppCore .NET project.

using System;

namespace AppCore.Extensions.Http.Authentication.OAuth;

internal static class AuthenticationSchemeExtensions
{
    public static void EnsureClientScheme(this AuthenticationScheme scheme)
    {
        if (scheme.HandlerType != typeof(OAuthClientAuthenticationHandler))
            throw new InvalidOperationException($"The client authentication scheme {scheme.Name} is not registered for the OAuth client handler.");
    }

    public static void EnsurePasswordScheme(this AuthenticationScheme scheme)
    {
        if (scheme.HandlerType != typeof(OAuthPasswordAuthenticationHandler))
            throw new InvalidOperationException($"The client authentication scheme {scheme.Name} is not registered for the OAuth password handler.");
    }
}