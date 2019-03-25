// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

namespace AppCore.DependencyInjection.Facilities
{
    /// <summary>
    /// Represents a type used to register extension services for a facility.
    /// </summary>
    /// <seealso cref="IFacility"/>
    /// <seealso cref="IFacilityExtension{TFacility}"/>
    public interface IFacilityExtension
    {
    }

    /// <summary>
    /// Represents a type used to register extension services for a facility.
    /// </summary>
    /// <typeparam name="TFacility"></typeparam>
    /// <seealso cref="IFacility"/>
    /// <seealso cref="FacilityExtension{TFacility}"/>
    public interface IFacilityExtension<in TFacility> : IFacilityExtension
        where TFacility : IFacility
    {
        /// <summary>
        /// Invoked to register components of the facility extension.
        /// </summary>
        void RegisterComponents(IComponentRegistry registry, TFacility facility);
    }
}