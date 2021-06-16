// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System.Collections.Generic;
using AppCore.DependencyInjection.Activator;

namespace AppCore.DependencyInjection.Facilities
{
    /// <summary>
    /// Represents a dynamic resolver for facilities.
    /// </summary>
    public interface IFacilityResolver
    {
        /// <summary>
        /// Resolves facilities.
        /// </summary>
        /// <returns>The <see cref="IEnumerable{T}"/> of facilities.</returns>
        IEnumerable<Facility> Resolve(IActivator activator);
    }
}