// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AppCoreNet.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;

namespace AppCore.Extensions.DependencyInjection;

/// <summary>
/// Builds an <see cref="IEnumerable{T}"/> of <see cref="ServiceDescriptor"/> by scanning assemblies in a
/// <see cref="DependencyContext"/>.
/// </summary>
public class DependencyContextServiceDescriptorResolver : IServiceDescriptorResolver
{
    private readonly AssemblyServiceDescriptorResolver _source;

    /// <summary>
    /// Initializes a new instance of the <see cref="DependencyContextServiceDescriptorResolver"/> class.
    /// </summary>
    public DependencyContextServiceDescriptorResolver()
    {
        _source = new AssemblyServiceDescriptorResolver();
    }

    /// <summary>
    /// Specifies whether to include private types when scanning for components.
    /// </summary>
    /// <param name="value">A value indicating whether to include private types.</param>
    /// <returns>The <see cref="DependencyContextServiceDescriptorResolver"/>.</returns>
    public DependencyContextServiceDescriptorResolver WithPrivateTypes(bool value = true)
    {
        _source.WithPrivateTypes(value);
        return this;
    }

    /// <summary>
    /// Adds an <see cref="DependencyContext"/> to be scanned for components.
    /// </summary>
    /// <param name="dependencyContext">The <see cref="DependencyContext"/>.</param>
    /// <returns>The <see cref="DependencyContextServiceDescriptorResolver"/>.</returns>
    public DependencyContextServiceDescriptorResolver Add(DependencyContext dependencyContext)
    {
        Ensure.Arg.NotNull(dependencyContext);

        _source.Add(
            dependencyContext.GetDefaultAssemblyNames()
                             .Select(Assembly.Load));

        return this;
    }

    /// <summary>
    /// Adds a type filter.
    /// </summary>
    /// <param name="filter">The type filter.</param>
    /// <returns>The <see cref="DependencyContextServiceDescriptorResolver"/>.</returns>
    public DependencyContextServiceDescriptorResolver Filter(Predicate<Type> filter)
    {
        _source.Filter(filter);
        return this;
    }

    /// <summary>
    /// Clears the current type filters.
    /// </summary>
    /// <returns>The <see cref="DependencyContextServiceDescriptorResolver"/>.</returns>
    public DependencyContextServiceDescriptorResolver ClearFilters()
    {
        _source.ClearFilters();
        return this;
    }

    /// <summary>
    /// Clears the assembly scanner default type filters.
    /// </summary>
    /// <returns>The <see cref="DependencyContextServiceDescriptorResolver"/>.</returns>
    public DependencyContextServiceDescriptorResolver ClearDefaultFilters()
    {
        _source.ClearDefaultFilters();
        return this;
    }

    /// <inheritdoc />
    IEnumerable<ServiceDescriptor> IServiceDescriptorResolver.Resolve(Type serviceType, ServiceLifetime defaultLifetime)
    {
        Ensure.Arg.NotNull(serviceType);
        return ((IServiceDescriptorResolver)_source).Resolve(serviceType, defaultLifetime);
    }
}