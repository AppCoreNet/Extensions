// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;

namespace AppCoreNet.Extensions.DependencyInjection.Activator;

/// <summary>
/// Helper code for the various activator services.
/// </summary>
public interface IActivator
{
    /// <summary>
    /// Instantiate a type with constructor arguments provided directly and/or from the <see cref="IServiceProvider"/>.
    /// </summary>
    /// <param name="instanceType">The type to activate.</param>
    /// <param name="parameters">Constructor arguments not provided by the <see cref="IServiceProvider"/>.</param>
    /// <returns>An activated object of type <paramref name="instanceType"/>.</returns>
    /// <exception cref="ArgumentNullException">Argument <paramref name="instanceType"/> or <paramref name="parameters"/> is null.</exception>
    object? CreateInstance(Type instanceType, params object[] parameters);
}