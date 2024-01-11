// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Collections.Generic;
using AppCoreNet.Diagnostics;

namespace AppCoreNet.Extensions.Http.Authentication;

/// <summary>
/// Represents the parameters used during authentication.
/// </summary>
public class AuthenticationParameters
{
    private IDictionary<string, object> _parameters;

    /// <summary>
    /// Gets or sets a value indicating whether to force renewing the authentication.
    /// </summary>
    public bool ForceRenewal
    {
        get => GetParameter<bool>(nameof(ForceRenewal));
        set => SetParameter(nameof(ForceRenewal), value);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthenticationParameters"/> class.
    /// </summary>
    protected AuthenticationParameters()
    {
        _parameters = new Dictionary<string, object>(StringComparer.Ordinal);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthenticationParameters"/> class.
    /// </summary>
    /// <param name="parameters">The initial parameters.</param>
    protected AuthenticationParameters(IDictionary<string, object> parameters)
    {
        Ensure.Arg.NotNull(parameters);
        _parameters = new Dictionary<string, object>(parameters, StringComparer.Ordinal);
    }

    /// <summary>
    /// Clones the authentication parameters.
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="AuthenticationParameters"/>.</typeparam>
    /// <returns>The cloned <see cref="AuthenticationParameters"/>.</returns>
    public T Clone<T>()
        where T : AuthenticationParameters, new()
    {
        var clone = new T
        {
            _parameters = new Dictionary<string, object>(_parameters, StringComparer.Ordinal),
        };

        return clone;
    }

    /// <summary>
    /// Gets a parameter.
    /// </summary>
    /// <typeparam name="T">Parameter type.</typeparam>
    /// <param name="key">Parameter key.</param>
    /// <returns>Retrieved value or the default value if the property is not set.</returns>
    protected T? GetParameter<T>(string key)
        => _parameters.TryGetValue(key, out object? obj) && obj is T value ? value : default;

    /// <summary>
    /// Sets a parameter.
    /// </summary>
    /// <typeparam name="T">Parameter type.</typeparam>
    /// <param name="key">Parameter key.</param>
    /// <param name="value">Value to set.</param>
    protected void SetParameter<T>(string key, T value)
        => _parameters[key] = value!;
}