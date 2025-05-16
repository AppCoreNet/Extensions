// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace AppCoreNet.Extensions.DependencyInjection.Facilities;

/// <summary>
/// Represents a builder for resolving facilities by reflection.
/// </summary>
public interface IFacilityReflectionBuilder
{
    /// <summary>
    /// Adds a facility resolver to the builder.
    /// </summary>
    /// <param name="resolver">The facility resolver.</param>
    /// <returns>The <see cref="IFacilityReflectionBuilder"/> to allow chaining.</returns>
    /// <exception cref="ArgumentNullException">Argument <paramref name="resolver"/> is null.</exception>
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    IFacilityReflectionBuilder AddResolver(IFacilityResolver resolver);

    /// <summary>
    /// Adds a facility resolver to the builder.
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="IFacilityResolver"/>.</typeparam>
    /// <param name="configure">The configuration delegate.</param>
    /// <returns>The <see cref="IFacilityReflectionBuilder"/> to allow chaining.</returns>
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public IFacilityReflectionBuilder AddResolver<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(Action<T>? configure = null)
        where T : IFacilityResolver;

    /// <summary>
    /// Adds extensions to the resolved facilities by reflection.
    /// </summary>
    /// <param name="configure">The configuration delegate.</param>
    /// <returns>The <see cref="IFacilityReflectionBuilder"/> to allow chaining.</returns>
    /// <exception cref="ArgumentNullException">Argument <paramref name="configure"/> is null.</exception>
    public IFacilityReflectionBuilder AddExtensionsFrom(Action<IFacilityExtensionReflectionBuilder> configure);
}