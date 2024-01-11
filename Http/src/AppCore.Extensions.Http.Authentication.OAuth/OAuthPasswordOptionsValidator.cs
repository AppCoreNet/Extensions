// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System.Collections.Generic;
using Microsoft.Extensions.Options;

namespace AppCore.Extensions.Http.Authentication.OAuth;

internal sealed class OAuthPasswordOptionsValidator
    : OAuthOptionsValidator,
      IValidateOptions<OAuthPasswordOptions>
{
    public ValidateOptionsResult Validate(string name, OAuthPasswordOptions options)
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