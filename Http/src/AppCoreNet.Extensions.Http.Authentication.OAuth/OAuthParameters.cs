// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using IdentityModel.Client;

namespace AppCoreNet.Extensions.Http.Authentication.OAuth;

/// <summary>
/// Represents the parameters used during OAuth authentication.
/// </summary>
public class OAuthParameters : AuthenticationParameters
{
    /// <summary>
    /// Gets or sets the resource parameter.
    /// </summary>
    public string? Resource
    {
        get => GetParameter<string>(nameof(Resource));
        set => SetParameter(nameof(Resource), value);
    }

    /// <summary>
    /// Gets or sets additional context that might be relevant in the pipeline.
    /// </summary>
    public Parameters? Context
    {
        get => GetParameter<Parameters>(nameof(Context));
        set => SetParameter(nameof(Context), value);
    }
}