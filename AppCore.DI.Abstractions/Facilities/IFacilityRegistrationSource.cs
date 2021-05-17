// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System.Collections.Generic;
using System.ComponentModel;

namespace AppCore.DependencyInjection.Facilities
{
    /// <summary>
    /// Represents a source for facility registrations.
    /// </summary>
    public interface IFacilityRegistrationSource
    {
        /// <summary>
        /// Gets the facilities which will be registered.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="Facility"/>.</returns>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        IEnumerable<Facility> GetFacilities();
    }
}