// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace AppCoreNet.Extensions.DependencyInjection.Facilities;

/// <summary>
/// Represents a builder for resolving facility extensions by reflection.
/// </summary>
public interface IFacilityExtensionReflectionBuilder
{
    /// <summary>
    /// Adds a facility extension resolver to the builder.
    /// </summary>
    /// <param name="resolver">The facility extension resolver.</param>
    /// <returns>The <see cref="IFacilityExtensionReflectionBuilder"/> to allow chaining.</returns>
    /// <exception cref="ArgumentNullException">Argument <paramref name="resolver"/> is null.</exception>
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    IFacilityExtensionReflectionBuilder AddResolver(IFacilityExtensionResolver resolver);

    /// <summary>
    /// Adds a facility extension resolver to the builder.
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="IFacilityExtensionResolver"/>.</typeparam>
    /// <param name="configure">The configuration delegate.</param>
    /// <returns>The <see cref="IFacilityExtensionReflectionBuilder"/> to allow chaining.</returns>
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public IFacilityExtensionReflectionBuilder AddResolver<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(Action<T>? configure = null)
        where T : IFacilityExtensionResolver;
}