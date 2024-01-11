// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AppCoreNet.Diagnostics;
using AppCore.Extensions.DependencyInjection.Activator;

namespace AppCore.Extensions.DependencyInjection.Facilities;

/// <summary>
/// Builds an <see cref="IEnumerable{T}"/> of <see cref="IFacility"/> by scanning assemblies.
/// </summary>
public class AssemblyResolver : IFacilityResolver, IFacilityExtensionResolver
{
    private readonly IActivator _activator;
    private readonly List<Assembly> _assemblies = new ();
    private readonly List<Predicate<Type>> _filters = new ();
    private bool _clearFilters;
    private bool _withPrivateTypes;

    /// <summary>
    /// Initializes a new instance of the <see cref="AssemblyResolver"/> class.
    /// </summary>
    /// <param name="activator">The <see cref="IActivator"/>.</param>
    /// <exception cref="ArgumentNullException">Argument <paramref name="activator"/> is <c>null</c>.</exception>
    public AssemblyResolver(IActivator activator)
    {
        Ensure.Arg.NotNull(activator);
        _activator = activator;
    }

    /// <summary>
    /// Specifies whether to include private types when scanning for facilities.
    /// </summary>
    /// <param name="value">A value indicating whether to include private types.</param>
    /// <returns>The <see cref="AssemblyResolver"/>.</returns>
    public AssemblyResolver WithPrivateTypes(bool value = true)
    {
        _withPrivateTypes = value;
        return this;
    }

    /// <summary>
    /// Adds an <see cref="Assembly"/> to be scanned for facilities.
    /// </summary>
    /// <param name="assembly">The <see cref="Assembly"/>.</param>
    /// <returns>The <see cref="AssemblyResolver"/>.</returns>
    /// <exception cref="ArgumentNullException">Argument <paramref name="assembly"/> is <c>null</c>.</exception>
    public AssemblyResolver Add(Assembly assembly)
    {
        Ensure.Arg.NotNull(assembly);
        _assemblies.Add(assembly);
        return this;
    }

    /// <summary>
    /// Adds an <see cref="IEnumerable{T}"/> of <see cref="Assembly"/> to be scanned for facilities.
    /// </summary>
    /// <param name="assemblies">The <see cref="IEnumerable{T}"/> of <see cref="Assembly"/>.</param>
    /// <returns>The <see cref="AssemblyResolver"/>.</returns>
    /// <exception cref="ArgumentNullException">Argument <paramref name="assemblies"/> is <c>null</c>.</exception>
    public AssemblyResolver Add(IEnumerable<Assembly> assemblies)
    {
        Ensure.Arg.NotNull(assemblies);
        _assemblies.AddRange(assemblies);
        return this;
    }

    /// <summary>
    /// Adds a type filter.
    /// </summary>
    /// <param name="filter">The type filter.</param>
    /// <returns>The <see cref="AssemblyResolver"/>.</returns>
    /// <exception cref="ArgumentNullException">Argument <paramref name="filter"/> is <c>null</c>.</exception>
    public AssemblyResolver Filter(Predicate<Type> filter)
    {
        Ensure.Arg.NotNull(filter);
        _filters.Add(filter);
        return this;
    }

    /// <summary>
    /// Clears the current type filters.
    /// </summary>
    /// <returns>The <see cref="AssemblyResolver"/>.</returns>
    public AssemblyResolver ClearFilters()
    {
        _filters.Clear();
        return this;
    }

    /// <summary>
    /// Clears the assembly scanner default type filters.
    /// </summary>
    /// <returns>The <see cref="AssemblyResolver"/>.</returns>
    public AssemblyResolver ClearDefaultFilters()
    {
        _clearFilters = true;
        return this;
    }

    /// <inheritdoc />
    IEnumerable<IFacility> IFacilityResolver.Resolve()
    {
        var scanner = new AssemblyScanner(typeof(IFacility), _assemblies)
        {
            IncludePrivateTypes = _withPrivateTypes,
        };

        if (_clearFilters)
            scanner.Filters.Clear();

        foreach (Predicate<Type> filter in _filters)
            scanner.Filters.Add(filter);

        return scanner.ScanAssemblies()
                      .Select(facilityType => (IFacility)_activator.CreateInstance(facilityType));
    }

    private IFacilityExtension<IFacility> CreateExtension(Type extensionType)
    {
        Type contractType = extensionType.GetInterfaces()
                                         .First(i => i.GetGenericTypeDefinition() == typeof(IFacilityExtension<>))
                                         .GenericTypeArguments[0];

        Type extensionWrapperType = typeof(FacilityExtensionWrapper<>).MakeGenericType(contractType);
        object extension = _activator.CreateInstance(extensionType);

        return (IFacilityExtension<IFacility>)System.Activator.CreateInstance(extensionWrapperType, extension);
    }

    /// <inheritdoc />
    IEnumerable<IFacilityExtension<IFacility>> IFacilityExtensionResolver.Resolve(Type facilityType)
    {
        Ensure.Arg.NotNull(facilityType);

        var scanner = new AssemblyScanner(typeof(IFacilityExtension<>).MakeGenericType(facilityType), _assemblies)
        {
            IncludePrivateTypes = _withPrivateTypes,
        };

        if (_clearFilters)
            scanner.Filters.Clear();

        foreach (Predicate<Type> filter in _filters)
            scanner.Filters.Add(filter);

        return scanner.ScanAssemblies()
                      .Select(CreateExtension);
    }
}