// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;

namespace AppCore.Extensions.Http.Authentication.OAuth;

internal static class AuthenticationSchemeExtensions
{
    public static void EnsureClientScheme(this AuthenticationScheme scheme)
    {
        if (scheme.HandlerType != typeof(OAuthClientHandler))
            throw new InvalidOperationException($"The client authentication scheme {scheme.Name} is not registered for the OAuth client handler.");
    }

    public static void EnsurePasswordScheme(this AuthenticationScheme scheme)
    {
        if (scheme.HandlerType != typeof(OAuthPasswordHandler))
            throw new InvalidOperationException($"The client authentication scheme {scheme.Name} is not registered for the OAuth password handler.");
    }
}