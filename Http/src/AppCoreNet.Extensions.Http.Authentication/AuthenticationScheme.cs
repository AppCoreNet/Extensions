// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using AppCoreNet.Diagnostics;

namespace AppCore.Extensions.Http.Authentication;

/// <summary>
/// Represents a registered authentication scheme.
/// </summary>
public sealed class AuthenticationScheme
{
    /// <summary>
    /// Gets the name of the authentication scheme.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the type of the <see cref="IAuthenticationSchemeHandler{TParameters}"/> which handles the scheme.
    /// </summary>
    public Type HandlerType { get; }

    /// <summary>
    /// Gets the type of the <see cref="AuthenticationSchemeOptions"/>.
    /// </summary>
    public Type OptionsType { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthenticationScheme"/> class.
    /// </summary>
    /// <param name="name">The name of the authentication scheme.</param>
    /// <param name="handlerType">The type of the <see cref="IAuthenticationSchemeHandler{TParameters}"/> which handles the scheme.</param>
    /// <param name="optionsType">The type of the <see cref="AuthenticationSchemeOptions"/>.</param>
    public AuthenticationScheme(string name, Type handlerType, Type optionsType)
    {
        Ensure.Arg.NotNull(name);
        Ensure.Arg.NotNull(handlerType);
        Ensure.Arg.OfType(handlerType, typeof(IAuthenticationSchemeHandler<>));
        Ensure.Arg.NotNull(optionsType);
        Ensure.Arg.OfType(optionsType, typeof(AuthenticationSchemeOptions));

        Name = name;
        HandlerType = handlerType;
        OptionsType = optionsType;
    }
}