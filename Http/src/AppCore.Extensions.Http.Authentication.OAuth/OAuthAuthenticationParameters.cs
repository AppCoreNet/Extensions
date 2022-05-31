// Licensed under the MIT License.
// Copyright (c) 2018-2022 the AppCore .NET project.

using IdentityModel.Client;

namespace AppCore.Extensions.Http.Authentication.OAuth;

/// <summary>
/// Represents the parameters used during OAuth authentication.
/// </summary>
public class OAuthAuthenticationParameters : AuthenticationParameters
{
    /// <summary>
    /// Specifies the resource parameter.
    /// </summary>
    public string? Resource
    {
        get => GetParameter<string>(nameof(Resource));
        set => SetParameter(nameof(Resource), value);
    }

    /// <summary>
    /// Additional context that might be relevant in the pipeline
    /// </summary>
    public Parameters? Context
    {
        get => GetParameter<Parameters>(nameof(Context));
        set => SetParameter(nameof(Context), value);
    }
}