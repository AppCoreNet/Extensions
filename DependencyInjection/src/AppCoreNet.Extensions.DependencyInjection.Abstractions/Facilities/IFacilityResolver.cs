// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System.Collections.Generic;

namespace AppCoreNet.Extensions.DependencyInjection.Facilities;

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