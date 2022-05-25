// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;

namespace AppCore.Extensions.DependencyInjection.Activator
{
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
        /// <returns>An activated object of type instanceType</returns>
        object CreateInstance(Type instanceType, params object[] parameters);
    }
}