// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using AppCoreNet.Diagnostics;

namespace AppCoreNet.Extensions.DependencyInjection.Activator;

/// <summary>
/// Provides extension methods for the <see cref="IActivator"/> interface.
/// </summary>
public static class ActivatorExtensions
{
    /// <summary>
    /// Instantiate a type with constructor arguments provided directly and/or from the <see cref="IServiceProvider"/>.
    /// </summary>
    /// <typeparam name="T">The type to activate.</typeparam>
    /// <param name="activator">The <see cref="IActivator"/>.</param>
    /// <param name="parameters">Constructor arguments not provided by the <see cref="IServiceProvider"/>.</param>
    /// <returns>An activated object of type <typeparamref name="T"/>.</returns>
    /// <exception cref="ArgumentNullException">Argument <paramref name="activator"/> is null.</exception>
    public static T? CreateInstance<T>(this IActivator activator, params object[] parameters)
    {
        Ensure.Arg.NotNull(activator);
        return (T?)activator.CreateInstance(typeof(T), parameters);
    }
}