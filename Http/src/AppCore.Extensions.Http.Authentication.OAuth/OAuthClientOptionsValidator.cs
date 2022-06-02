// Licensed under the MIT License.
// Copyright (c) 2018-2022 the AppCore .NET project.

using System.Collections.Generic;
using Microsoft.Extensions.Options;

namespace AppCore.Extensions.Http.Authentication.OAuth;

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