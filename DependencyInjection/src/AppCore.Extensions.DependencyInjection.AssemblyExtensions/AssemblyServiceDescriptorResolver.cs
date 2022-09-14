// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Reflection;
using AppCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace AppCore.Extensions.DependencyInjection;

/// <summary>
/// Builds an <see cref="IEnumerable{T}"/> of <see cref="ServiceDescriptor"/> by scanning assemblies.
/// </summary>
public class AssemblyServiceDescriptorResolver : IServiceDescriptorResolver
{
    private readonly List<Assembly> _assemblies = new();
    private readonly List<Predicate<Type>> _filters = new();
    private bool _clearFilters;
    private bool _withPrivateTypes;

    /// <summary>
    /// Initializes a new instance of the <see cref="AssemblyServiceDescriptorResolver"/> class.
    /// </summary>
    public AssemblyServiceDescriptorResolver()
    {
    }

    /// <summary>
    /// Specifies whether to include private types when scanning for services.
    /// </summary>
    /// <param name="value">A value indicating whether to include private types.</param>
    /// <returns>The <see cref="AssemblyServiceDescriptorResolver"/>.</returns>
    public AssemblyServiceDescriptorResolver WithPrivateTypes(bool value = true)
    {
        _withPrivateTypes = value;
        return this;
    }

    /// <summary>
    /// Adds an <see cref="Assembly"/> to be scanned for services.
    /// </summary>
    /// <param name="assembly">The <see cref="Assembly"/>.</param>
    /// <returns>The <see cref="AssemblyServiceDescriptorResolver"/>.</returns>
    public AssemblyServiceDescriptorResolver Add(Assembly assembly)
    {
        Ensure.Arg.NotNull(assembly);
        _assemblies.Add(assembly);
        return this;
    }

    /// <summary>
    /// Adds an <see cref="IEnumerable{T}"/> of <see cref="Assembly"/> to be scanned for services.
    /// </summary>
    /// <param name="assemblies">The <see cref="IEnumerable{T}"/> of <see cref="Assembly"/>.</param>
    /// <returns>The <see cref="AssemblyServiceDescriptorResolver"/>.</returns>
    public AssemblyServiceDescriptorResolver Add(IEnumerable<Assembly> assemblies)
    {
        Ensure.Arg.NotNull(assemblies);
        _assemblies.AddRange(assemblies);
        return this;
    }

    /// <summary>
    /// Adds a type filter.
    /// </summary>
    /// <param name="filter">The type filter.</param>
    /// <returns>The <see cref="AssemblyServiceDescriptorResolver"/>.</returns>
    public AssemblyServiceDescriptorResolver Filter(Predicate<Type> filter)
    {
        Ensure.Arg.NotNull(filter);
        _filters.Add(filter);
        return this;
    }

    /// <summary>
    /// Clears the current type filters.
    /// </summary>
    /// <returns>The <see cref="AssemblyServiceDescriptorResolver"/>.</returns>
    public AssemblyServiceDescriptorResolver ClearFilters()
    {
        _filters.Clear();
        return this;
    }

    /// <summary>
    /// Clears the assembly resolver default type filters.
    /// </summary>
    /// <returns>The <see cref="AssemblyServiceDescriptorResolver"/>.</returns>
    public AssemblyServiceDescriptorResolver ClearDefaultFilters()
    {
        _clearFilters = true;
        return this;
    }

    /// <inheritdoc />
    IEnumerable<ServiceDescriptor> IServiceDescriptorResolver.Resolve(Type serviceType, ServiceLifetime defaultLifetime)
    {
        ServiceLifetime GetServiceLifetime(Type implementationType)
        {
            var lifetimeAttribute =
                implementationType.GetTypeInfo()
                                  .GetCustomAttribute<LifetimeAttribute>();

            return lifetimeAttribute?.Lifetime ?? defaultLifetime;
        }

        var scanner = new AssemblyScanner(serviceType, _assemblies)
        {
            IncludePrivateTypes = _withPrivateTypes
        };

        if (_clearFilters)
            scanner.Filters.Clear();

        foreach (Predicate<Type> filter in _filters)
            scanner.Filters.Add(filter);

        IEnumerable<Type> implementationTypes = scanner.ScanAssemblies();
        bool isOpenGenericContractType = scanner.ContractType.IsGenericTypeDefinition;

        foreach (Type implementationType in implementationTypes)
        {
            bool isOpenGenericType = implementationType.GetTypeInfo()
                                                       .IsGenericTypeDefinition;

            // skip non-closed generic component types if contract type is not a open generic type
            if (!isOpenGenericContractType && isOpenGenericType)
                continue;

            // need to register closed types with closed generic contract type
            Type currentServiceType = serviceType;
            if (isOpenGenericContractType && !isOpenGenericType)
                currentServiceType = implementationType.GetClosedTypeOf(currentServiceType);

            yield return ServiceDescriptor.Describe(
                currentServiceType,
                implementationType,
                GetServiceLifetime(implementationType));
        }
    }
}