// Licensed under the MIT License.
// Copyright (c) 2018-2022 the AppCore .NET project.

using System;
using AppCore.Diagnostics;

namespace AppCore.Extensions.Http.Authentication;

/// <summary>
/// Represents a registered authentication scheme.
/// </summary>
public class AuthenticationScheme
{
    /// <summary>
    /// The name of the authentication scheme.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// The type of the <see cref="IAuthenticationSchemeHandler{TParameters}"/> which handles the scheme.
    /// </summary>
    public Type HandlerType { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthenticationScheme"/> class.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="handlerType"></param>
    public AuthenticationScheme(string name, Type handlerType)
    {
        Ensure.Arg.NotNull(name);
        Ensure.Arg.NotNull(handlerType);
        Ensure.Arg.OfType(handlerType, typeof(IAuthenticationSchemeHandler<>));

        Name = name;
        HandlerType = handlerType;
    }
}