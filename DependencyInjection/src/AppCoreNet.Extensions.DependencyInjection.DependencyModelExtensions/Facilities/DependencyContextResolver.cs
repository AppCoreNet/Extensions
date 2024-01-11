// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AppCoreNet.Diagnostics;
using AppCoreNet.Extensions.DependencyInjection.Activator;
using Microsoft.Extensions.DependencyModel;

// ReSharper disable once CheckNamespace
namespace AppCoreNet.Extensions.DependencyInjection.Facilities;

/// <summary>
/// Builds an <see cref="IEnumerable{T}"/> of <see cref="IFacility"/> by scanning assemblies in a
/// <see cref="DependencyContext"/>.
/// </summary>
public class DependencyContextResolver : IFacilityResolver, IFacilityExtensionResolver
{
    private readonly AssemblyResolver _resolver;

    /// <summary>
    /// Initializes a new instance of the <see cref="DependencyContextResolver"/> class.
    /// </summary>
    /// <param name="activator">The <see cref="IActivator"/>.</param>
    /// <exception cref="ArgumentNullException">Argument <paramref name="activator"/> is <c>null</c>.</exception>
    public DependencyContextResolver(IActivator activator)
    {
        _resolver = new AssemblyResolver(activator);
    }

    /// <summary>
    /// Specifies whether to include private types when scanning for components.
    /// </summary>
    /// <param name="value">A value indicating whether to include private types.</param>
    /// <returns>The <see cref="DependencyContextResolver"/>.</returns>
    public DependencyContextResolver WithPrivateTypes(bool value = true)
    {
        _resolver.WithPrivateTypes(value);
        return this;
    }

    /// <summary>
    /// Adds an <see cref="DependencyContext"/> to be scanned for components.
    /// </summary>
    /// <param name="dependencyContext">The <see cref="DependencyContext"/>.</param>
    /// <returns>The <see cref="DependencyContextResolver"/>.</returns>
    /// <exception cref="ArgumentNullException">Argument <paramref name="dependencyContext"/> is <c>null</c>.</exception>
    public DependencyContextResolver Add(DependencyContext dependencyContext)
    {
        Ensure.Arg.NotNull(dependencyContext);

        _resolver.Add(
            dependencyContext.GetDefaultAssemblyNames()
                             .Select(Assembly.Load));

        return this;
    }

    /// <summary>
    /// Adds a type filter.
    /// </summary>
    /// <param name="filter">The type filter.</param>
    /// <returns>The <see cref="DependencyContextResolver"/>.</returns>
    /// <exception cref="ArgumentNullException">Argument <paramref name="filter"/> is <c>null</c>.</exception>
    public DependencyContextResolver Filter(Predicate<Type> filter)
    {
        _resolver.Filter(filter);
        return this;
    }

    /// <summary>
    /// Clears the current type filters.
    /// </summary>
    /// <returns>The <see cref="DependencyContextResolver"/>.</returns>
    public DependencyContextResolver ClearFilters()
    {
        _resolver.ClearFilters();
        return this;
    }

    /// <summary>
    /// Clears the assembly scanner default type filters.
    /// </summary>
    /// <returns>The <see cref="DependencyContextResolver"/>.</returns>
    public DependencyContextResolver ClearDefaultFilters()
    {
        _resolver.ClearDefaultFilters();
        return this;
    }

    /// <inheritdoc />
    IEnumerable<IFacility> IFacilityResolver.Resolve()
    {
        return ((IFacilityResolver)_resolver).Resolve();
    }

    /// <inheritdoc />
    IEnumerable<IFacilityExtension<IFacility>> IFacilityExtensionResolver.Resolve(Type facilityType)
    {
        return ((IFacilityExtensionResolver)_resolver).Resolve(facilityType);
    }
}