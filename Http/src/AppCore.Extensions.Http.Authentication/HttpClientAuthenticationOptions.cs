﻿// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Collections.Generic;

namespace AppCore.Extensions.Http.Authentication;

/// <summary>
/// Represents the options for HTTP client authentication.
/// </summary>
public sealed class HttpClientAuthenticationOptions
{
    /// <summary>
    /// Gets the dictionary of registered authentication schemes.
    /// </summary>
    public IDictionary<string, AuthenticationScheme> SchemeMap { get; }
        = new Dictionary<string, AuthenticationScheme>(StringComparer.Ordinal);

    /// <summary>
    /// Adds a authentication scheme.
    /// </summary>
    /// <param name="name">The name of the authentication scheme.</param>
    /// <param name="handlerType">The type of the <see cref="IAuthenticationSchemeHandler{TParameters}"/>.</param>
    /// <param name="optionsType">The type of the <see cref="AuthenticationSchemeOptions"/>.</param>
    public void AddScheme(string name, Type handlerType, Type optionsType)
        => SchemeMap.Add(name, new AuthenticationScheme(name, handlerType, optionsType));

    /// <summary>
    /// Adds a authentication scheme.
    /// </summary>
    /// <param name="scheme">The <see cref="AuthenticationScheme"/>.</param>
    public void AddScheme(AuthenticationScheme scheme)
        => SchemeMap.Add(scheme.Name, scheme);
}