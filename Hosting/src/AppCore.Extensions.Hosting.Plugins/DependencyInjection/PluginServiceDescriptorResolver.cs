// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Linq;
using AppCoreNet.Diagnostics;
using AppCore.Extensions.Hosting.Plugins;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

// ReSharper disable once CheckNamespace
namespace AppCore.Extensions.DependencyInjection;

/// <summary>
/// Builds an <see cref="IEnumerable{T}"/> of <see cref="ServiceDescriptor"/> by scanning plugin assemblies.
/// </summary>
public class PluginServiceDescriptorResolver : IServiceDescriptorResolver
{
    private readonly List<Predicate<Type>> _filters = new ();
    private readonly IPluginManager _pluginManager;
    private readonly IOptions<PluginOptions> _pluginOptions;

    /// <summary>
    /// Initializes a new instance of the <see cref="PluginServiceDescriptorResolver"/> class.
    /// </summary>
    /// <param name="pluginManager">The <see cref="IPluginManager"/>.</param>
    /// <param name="pluginOptions">The plugin options.</param>
    public PluginServiceDescriptorResolver(IPluginManager pluginManager, IOptions<PluginOptions> pluginOptions)
    {
        Ensure.Arg.NotNull(pluginManager);
        _pluginManager = pluginManager;
        _pluginOptions = pluginOptions;
    }

    /// <summary>
    /// Adds a type filter.
    /// </summary>
    /// <param name="filter">The type filter.</param>
    /// <returns>The <see cref="PluginServiceDescriptorResolver"/>.</returns>
    public PluginServiceDescriptorResolver Filter(Predicate<Type> filter)
    {
        Ensure.Arg.NotNull(filter);
        _filters.Add(filter);
        return this;
    }

    /// <summary>
    /// Clears the current type filters.
    /// </summary>
    /// <returns>The <see cref="PluginServiceDescriptorResolver"/>.</returns>
    public PluginServiceDescriptorResolver ClearFilters()
    {
        _filters.Clear();
        return this;
    }

    /// <inheritdoc />
    IEnumerable<ServiceDescriptor> IServiceDescriptorResolver.Resolve(Type serviceType, ServiceLifetime defaultLifetime)
    {
        var resolver = new AssemblyServiceDescriptorResolver();

        resolver.Add(_pluginManager.Plugins.Select(p => p.Assembly));
        resolver.WithPrivateTypes(_pluginOptions.Value.ResolvePrivateTypes);
        resolver.ClearDefaultFilters();
        foreach (Predicate<Type> filter in _filters)
        {
            resolver.Filter(filter);
        }

        return ((IServiceDescriptorResolver)resolver).Resolve(serviceType, defaultLifetime);
    }
}