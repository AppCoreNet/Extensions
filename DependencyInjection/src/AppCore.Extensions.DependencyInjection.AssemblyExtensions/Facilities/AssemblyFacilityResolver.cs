// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AppCore.Diagnostics;
using AppCore.Extensions.DependencyInjection.Activator;
using AppCore.Extensions.DependencyInjection.Facilities;

namespace AppCore.Extensions.DependencyInjection.Facilities;

/// <summary>
/// Builds an <see cref="IEnumerable{T}"/> of <see cref="Facility"/> by scanning assemblies.
/// </summary>
public class AssemblyFacilityResolver : IFacilityResolver
{
    private readonly IActivator _activator;
    private readonly List<Assembly> _assemblies = new();
    private readonly List<Predicate<Type>> _filters = new();
    private bool _clearFilters;
    private bool _withPrivateTypes;

    /// <summary>
    /// Initializes a new instance of the <see cref="AssemblyFacilityResolver"/> class.
    /// </summary>
    public AssemblyFacilityResolver(IActivator activator)
    {
        _activator = activator;
    }

    /// <summary>
    /// Specifies whether to include private types when scanning for facilities.
    /// </summary>
    /// <param name="value">A value indicating whether to include private types.</param>
    /// <returns>The <see cref="AssemblyFacilityResolver"/>.</returns>
    public AssemblyFacilityResolver WithPrivateTypes(bool value = true)
    {
        _withPrivateTypes = value;
        return this;
    }

    /// <summary>
    /// Adds an <see cref="Assembly"/> to be scanned for facilities.
    /// </summary>
    /// <param name="assembly">The <see cref="Assembly"/>.</param>
    /// <returns>The <see cref="AssemblyFacilityResolver"/>.</returns>
    public AssemblyFacilityResolver Add(Assembly assembly)
    {
        Ensure.Arg.NotNull(assembly);
        _assemblies.Add(assembly);
        return this;
    }

    /// <summary>
    /// Adds an <see cref="IEnumerable{T}"/> of <see cref="Assembly"/> to be scanned for facilities.
    /// </summary>
    /// <param name="assemblies">The <see cref="IEnumerable{T}"/> of <see cref="Assembly"/>.</param>
    /// <returns>The <see cref="AssemblyFacilityResolver"/>.</returns>
    public AssemblyFacilityResolver Add(IEnumerable<Assembly> assemblies)
    {
        Ensure.Arg.NotNull(assemblies);
        _assemblies.AddRange(assemblies);
        return this;
    }

    /// <summary>
    /// Adds a type filter.
    /// </summary>
    /// <param name="filter">The type filter.</param>
    /// <returns>The <see cref="AssemblyFacilityResolver"/>.</returns>
    public AssemblyFacilityResolver Filter(Predicate<Type> filter)
    {
        Ensure.Arg.NotNull(filter);
        _filters.Add(filter);
        return this;
    }

    /// <summary>
    /// Clears the current type filters.
    /// </summary>
    /// <returns>The <see cref="AssemblyFacilityResolver"/>.</returns>
    public AssemblyFacilityResolver ClearFilters()
    {
        _filters.Clear();
        return this;
    }

    /// <summary>
    /// Clears the assembly scanner default type filters.
    /// </summary>
    /// <returns>The <see cref="AssemblyFacilityResolver"/>.</returns>
    public AssemblyFacilityResolver ClearDefaultFilters()
    {
        _clearFilters = true;
        return this;
    }

    /// <inheritdoc />
    IEnumerable<Facility> IFacilityResolver.Resolve()
    {
        var scanner = new AssemblyScanner(typeof(Facility), _assemblies)
        {
            IncludePrivateTypes = _withPrivateTypes
        };

        if (_clearFilters)
            scanner.Filters.Clear();

        foreach (Predicate<Type> filter in _filters)
            scanner.Filters.Add(filter);

        return scanner.ScanAssemblies()
                      .Select(facilityType => (Facility) _activator.CreateInstance(facilityType));
    }
}