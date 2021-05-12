// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using System.Collections.Generic;

namespace AppCore.DependencyInjection
{
    /// <summary>
    /// Represents a type used to register components with a dependency injection container.
    /// </summary>
    /// <seealso cref="ComponentRegistration"/>
    public interface IComponentRegistry
    {
        /// <summary>
        /// Adds the specified component registrations to the registry.
        /// </summary>
        /// <param name="registrations">The enumerable of <see cref="ComponentRegistration"/> to add.</param>
        /// <returns>The <see cref="IComponentRegistry"/>.</returns>
        IComponentRegistry Add(IEnumerable<ComponentRegistration> registrations);

        /// <summary>
        /// Adds the specified component registrations to the registry if the contract of the
        /// component has not been already added.
        /// </summary>
        /// <param name="registrations">The enumerable of <see cref="ComponentRegistration"/> to add.</param>
        /// <returns>The <see cref="IComponentRegistry"/>.</returns>
        IComponentRegistry TryAdd(IEnumerable<ComponentRegistration> registrations);

        /// <summary>
        /// Adds the specified component registrations to the registry if the contract and implementation
        /// type of the component has not been already added.
        /// </summary>
        /// <param name="registrations">The enumerable of <see cref="ComponentRegistration"/> to add.</param>
        /// <returns>The <see cref="IComponentRegistry"/>.</returns>
        IComponentRegistry TryAddEnumerable(IEnumerable<ComponentRegistration> registrations);
    }
}