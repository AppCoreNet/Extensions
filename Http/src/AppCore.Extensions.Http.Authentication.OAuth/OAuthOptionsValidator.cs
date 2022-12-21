// Licensed under the MIT License.
// Copyright (c) 2018-2022 the AppCore .NET project.

using System.Collections.Generic;

namespace AppCore.Extensions.Http.Authentication.OAuth;

internal abstract class OAuthOptionsValidator
{
    protected List<string> Validate(string name, OAuthOptions options)
    {
        List<string> errors = new();

        if (options.TokenEndpoint == null)
        {
            errors.Add($"Token endpoint for OAuth authentication scheme '{name}' must not be null.");
        }

        if (string.IsNullOrWhiteSpace(options.ClientId))
        {
            errors.Add($"ClientId for OAuth authentication scheme '{name}' must not be null or empty.");
        }

        if (string.IsNullOrWhiteSpace(options.ClientSecret))
        {
            errors.Add($"ClientSecret for OAuth authentication scheme '{name}' must not be null or empty.");
        }

        return errors;
    }
}