// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System.Collections.Generic;

namespace AppCore.DependencyInjection
{
    /// <summary>
    /// Represents a type used to register extension services for a facility.
    /// </summary>
    /// <typeparam name="TFacility"></typeparam>
    /// <seealso cref="IFacility"/>
    /// <seealso cref="FacilityExtension{TFacility}"/>
    public interface IFacilityExtension<in TFacility>
        where TFacility : IFacility
    {
        /// <summary>
        /// Returns the components registerd by the facility extension.
        /// </summary>
        IEnumerable<ComponentRegistration> GetComponentRegistrations(TFacility facility);
    }
}