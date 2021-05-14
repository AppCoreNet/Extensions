// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;

namespace AppCore.DependencyInjection.Activator
{
    /// <summary>
    /// Helper code for the various activator services.
    /// </summary>
    public interface IActivator
    {
        /// <summary>
        /// Instantiate a type with constructor arguments provided directly and/or from the <see cref="IContainer"/>.
        /// </summary>
        /// <param name="instanceType">The type to activate.</param>
        /// <param name="parameters">Constructor arguments not provided by the <see cref="IContainer"/>.</param>
        /// <returns>An activated object of type instanceType</returns>
        object CreateInstance(Type instanceType, params object[] parameters);

        /// <summary>
        /// Retrieve an instance of the given type from the container. If one is not found then instantiate it directly.
        /// </summary>
        /// <param name="type">The type of the service.</param>
        /// <returns>The resolved service or created instance.</returns>
        object ResolveOrCreateInstance(Type type);
    }
}