// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace AppCore.DependencyInjection
{
    /// <summary>
    /// Represents a source for component registrations.
    /// </summary>
    public interface IComponentRegistrationSource
    {
        /// <summary>
        /// Sets the contract type of the components.
        /// </summary>
        /// <param name="contractType">The type of the contract.</param>
        void WithContract(Type contractType);

        /// <summary>
        /// Specifies the default lifetime for components.
        /// </summary>
        /// <param name="lifetime">The default lifetime.</param>
        void WithDefaultLifetime(ComponentLifetime lifetime);

        /// <summary>
        /// Builds the component registrations.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="ComponentRegistration"/>.</returns>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        IEnumerable<ComponentRegistration> GetRegistrations();
    }
}