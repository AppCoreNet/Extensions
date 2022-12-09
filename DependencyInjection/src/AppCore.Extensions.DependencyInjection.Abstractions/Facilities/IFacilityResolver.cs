// Licensed under the MIT License.
// Copyright (c) 2018-2022 the AppCore .NET project.

using System.Collections.Generic;

namespace AppCore.Extensions.DependencyInjection.Facilities;

/// <summary>
/// Represents a dynamic resolver for facilities.
/// </summary>
public interface IFacilityResolver
{
    /// <summary>
    /// Resolves facilities.
    /// </summary>
    /// <returns>The <see cref="IEnumerable{T}"/> of facilities.</returns>
    IEnumerable<IFacility> Resolve();
}