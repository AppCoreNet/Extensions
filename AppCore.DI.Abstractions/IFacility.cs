// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System.Collections.Generic;

namespace AppCore.DependencyInjection
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
        /// Returns the components registerd by the facility.
        /// </summary>
        IEnumerable<ComponentRegistration> GetComponentRegistrations();
    }
}
