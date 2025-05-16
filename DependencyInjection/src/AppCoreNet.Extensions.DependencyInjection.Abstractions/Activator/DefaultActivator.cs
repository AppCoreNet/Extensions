// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Diagnostics.CodeAnalysis;
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
    public static DefaultActivator Instance { get; } = new();

    /// <inheritdoc />
    public object? CreateInstance(
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type instanceType,
        params object[] parameters)
    {
        Ensure.Arg.NotNull(instanceType);
        Ensure.Arg.NotNull(parameters);

        return System.Activator.CreateInstance(instanceType, parameters);
    }
}