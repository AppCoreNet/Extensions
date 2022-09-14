// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using AppCore.Diagnostics;

namespace AppCore.Extensions.DependencyInjection.Activator;

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
    /// <returns>An activated object of type instanceType</returns>
    public static T CreateInstance<T>(this IActivator activator, params object[] parameters)
    {
        Ensure.Arg.NotNull(activator);
        return (T) activator.CreateInstance(typeof(T), parameters);
    }
}