// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

namespace AppCore.DependencyInjection.Facilities
{
    /// <summary>
    /// Represents a type used to register a application facility with the dependency injection container.
    /// </summary>
    /// <seealso cref="Facility"/>
    /// <seealso cref="FacilityExtension{TFacility}"/>
    /// <seealso cref="IFacilityExtension{TFacility}"/>
    public interface IFacility
    {
        /// <summary>
        /// Invoked to register components of the facility.
        /// </summary>
        void RegisterComponents(IComponentRegistry registry);
    }
}
