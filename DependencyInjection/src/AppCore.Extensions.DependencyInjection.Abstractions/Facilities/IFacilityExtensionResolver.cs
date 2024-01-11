// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Collections.Generic;

namespace AppCore.Extensions.DependencyInjection.Facilities;

/// <summary>
/// Represents a dynamic resolver for facility extensions.
/// </summary>
public interface IFacilityExtensionResolver
{
    /// <summary>
    /// Resolves facility extensions.
    /// </summary>
    /// <param name="facilityType">The type of the facility.</param>
    /// <returns>The <see cref="IEnumerable{T}"/> of facility extensions.</returns>
    /// <exception cref="ArgumentNullException">Argument <paramref name="facilityType"/> is null.</exception>
    IEnumerable<IFacilityExtension<IFacility>> Resolve(Type facilityType);
}