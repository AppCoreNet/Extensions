// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using AppCore.Diagnostics;

namespace AppCore.DependencyInjection
{
    /// <summary>
    /// Provides extension methods for the <see cref="IActivator"/> interface.
    /// </summary>
    public static class ActivatorExtensions
    {
        /// <summary>
        /// Instantiate a type with constructor arguments provided directly and/or from the <see cref="IContainer"/>.
        /// </summary>
        /// <typeparam name="T">The type to activate.</typeparam>
        /// <param name="activator">The <see cref="IActivator"/>.</param>
        /// <param name="parameters">Constructor arguments not provided by the <see cref="IContainer"/>.</param>
        /// <returns>An activated object of type instanceType</returns>
        public static T CreateInstance<T>(this IActivator activator, params object[] parameters)
        {
            Ensure.Arg.NotNull(activator, nameof(activator));
            return (T) activator.CreateInstance(typeof(T), parameters);
        }

        /// <summary>
        /// Retrieve an instance of the given type from the container. If one is not found then instantiate it directly.
        /// </summary>
        /// <typeparam name="T">The type of the service.</typeparam>
        /// <param name="activator">The <see cref="IActivator"/>.</param>
        /// <returns>The resolved service or created instance.</returns>
        public static T ResolveOrCreateInstance<T>(this IActivator activator)
        {
            Ensure.Arg.NotNull(activator, nameof(activator));
            return (T) activator.ResolveOrCreateInstance(typeof(T));
        }
    }
}