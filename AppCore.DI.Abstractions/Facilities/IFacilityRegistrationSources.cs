// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using System.ComponentModel;

namespace AppCore.DependencyInjection.Facilities
{
    /// <summary>
    /// Represents a collection of <see cref="IFacilityRegistrationSource"/>.
    /// </summary>
    public interface IFacilityRegistrationSources
    {
        /// <summary>
        /// Adds a registration source to the collection.
        /// </summary>
        /// <param name="source">The component registration source.</param>
        /// <returns>The <see cref="IFacilityRegistrationSources"/> to allow chaining.</returns>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        IFacilityRegistrationSources Add(IFacilityRegistrationSource source);

        /// <summary>
        /// Adds a registration source to the collection.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IFacilityRegistrationSource"/>.</typeparam>
        /// <param name="configure">The configuration delegate.</param>
        /// <returns>The <see cref="IFacilityRegistrationSources"/> to allow chaining.</returns>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        IFacilityRegistrationSources Add<T>(Action<T> configure = null)
            where T : IFacilityRegistrationSource, new();
    }
}