// Licensed under the MIT License.
// Copyright (c) 2018-2022 the AppCore .NET project.

using System.Collections.Generic;
using Microsoft.Extensions.Options;

namespace AppCore.Extensions.Http.Authentication.OAuth;

internal sealed class ValidateOAuthPasswordAuthenticationOptions
    : ValidateOAuthAuthenticationOptions,
      IValidateOptions<OAuthPasswordAuthenticationOptions>
{
    public ValidateOptionsResult Validate(string name, OAuthPasswordAuthenticationOptions options)
    {
        List<string> errors = base.Validate(name, options);

        if (string.IsNullOrWhiteSpace(options.Username))
        {
            errors.Add($"Username for OAuth authentication scheme {name} must not be null or empty.");
        }

        if (string.IsNullOrWhiteSpace(options.Password))
        {
            errors.Add($"Password for OAuth authentication scheme {name} must not be null or empty.");
        }

        return errors.Count > 0
            ? ValidateOptionsResult.Fail(errors)
            : ValidateOptionsResult.Success;
    }
}