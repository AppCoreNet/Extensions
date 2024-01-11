// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using AppCoreNet.Diagnostics;

namespace AppCoreNet.Extensions.DependencyInjection.Activator;

/// <summary>
/// Represents the default activator.
/// </summary>
public sealed class DefaultActivator : IActivator
{
    /// <summary>
    /// Gets the static instance of the <see cref="DefaultActivator"/>.
    /// </summary>
    public static DefaultActivator Instance { get; } = new ();

    /// <inheritdoc />
    public object CreateInstance(Type instanceType, params object[] parameters)
    {
        Ensure.Arg.NotNull(instanceType);
        return System.Activator.CreateInstance(instanceType, parameters ?? Array.Empty<object>());
    }
}