// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace AppCore.DependencyInjection
{
    /// <summary>
    /// Represents a collection of <see cref="IComponentRegistrationSource"/>.
    /// </summary>
    public interface IComponentRegistrationSources
    {
        /// <summary>
        /// Adds a registration source to the collection.
        /// </summary>
        /// <param name="source">The component registration source.</param>
        /// <returns>The <see cref="IComponentRegistrationSources"/> to allow chaining.</returns>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        IComponentRegistrationSources Add(IComponentRegistrationSource source);

        /// <summary>
        /// Adds a registration source to the collection.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IComponentRegistrationSource"/>.</typeparam>
        /// <param name="configure">The configuration delegate.</param>
        /// <returns>The <see cref="IComponentRegistrationSources"/> to allow chaining.</returns>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        IComponentRegistrationSources Add<T>(Action<T> configure)
            where T : IComponentRegistrationSource, new();

        /// <summary>
        /// Builds the component registrations from all registered sources.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="ComponentRegistration"/>.</returns>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        IEnumerable<ComponentRegistration> GetRegistrations();
    }
}