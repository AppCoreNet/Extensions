// Licensed under the MIT License.
// Copyright (c) 2018-2022 the AppCore .NET project.

using System;

namespace AppCore.Extensions.Http.Authentication.OAuth.OpenIdConnect;

internal static class AuthenticationSchemeExtensions
{
    public static void EnsureOpenIdConnectScheme(this AuthenticationScheme scheme)
    {
        if (!typeof(OpenIdConnectUserHandler).IsAssignableFrom(scheme.HandlerType))
            throw new InvalidOperationException($"The client authentication scheme {scheme.Name} is not registered for the OpenID Connect user handler.");
    }
}