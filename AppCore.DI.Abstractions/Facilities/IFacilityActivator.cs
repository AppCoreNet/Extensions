// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;

namespace AppCore.DependencyInjection.Facilities
{
    /// <summary>
    /// Represents a factory for facilities.
    /// </summary>
    public interface IFacilityActivator
    {
        /// <summary>
        /// Creates an instance of a <see cref="Facility"/> or <see cref="FacilityExtension"/>.
        /// </summary>
        /// <param name="facilityType">The type of the <see cref="Facility"/> or <see cref="FacilityExtension"/>.</param>
        /// <returns>The instance of the facility.</returns>
        object CreateInstance(Type facilityType);
    }
}