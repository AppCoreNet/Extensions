// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace AppCore.DependencyInjection
{
    /// <summary>
    /// Represents a type used to register components with a dependency injection container.
    /// </summary>
    /// <seealso cref="ComponentRegistration"/>
    public interface IComponentRegistry
    {
        /// <summary>
        /// Registers a callback registering a component with the dependency injection container.
        /// </summary>
        /// <param name="registration">
        ///     Callback which returns the <see cref="ComponentRegistration"/> describing the component
        ///     to register.
        /// </param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        void RegisterCallback(Func<IEnumerable<ComponentRegistration>> registration);
    }
}