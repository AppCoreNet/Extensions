// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System.Collections.Generic;
using Microsoft.Extensions.Options;

namespace AppCoreNet.Extensions.Http.Authentication.OAuth;

internal sealed class OAuthClientOptionsValidator
    : OAuthOptionsValidator,
      IValidateOptions<OAuthClientOptions>
{
    public ValidateOptionsResult Validate(string name, OAuthClientOptions options)
    {
        List<string> errors = base.Validate(name, options);

        return errors.Count > 0
            ? ValidateOptionsResult.Fail(errors)
            : ValidateOptionsResult.Success;
    }
}