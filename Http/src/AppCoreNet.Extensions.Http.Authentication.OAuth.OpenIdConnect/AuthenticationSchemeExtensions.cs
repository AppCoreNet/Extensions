// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;

namespace AppCoreNet.Extensions.Http.Authentication.OAuth.OpenIdConnect;

internal static class AuthenticationSchemeExtensions
{
    public static void EnsureOpenIdConnectScheme(this AuthenticationScheme scheme)
    {
        if (!typeof(OpenIdConnectUserHandler).IsAssignableFrom(scheme.HandlerType))
            throw new InvalidOperationException($"The client authentication scheme {scheme.Name} is not registered for the OpenID Connect user handler.");
    }
}