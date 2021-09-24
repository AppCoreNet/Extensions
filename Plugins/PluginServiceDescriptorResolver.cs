// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace AppCore.DependencyInjection
{
    /// <summary>
    /// Builds an <see cref="IEnumerable{T}"/> of <see cref="ServiceDescriptor"/> by scanning plugin assemblies. 
    /// </summary>
    public class PluginServiceDescriptorResolver : IServiceDescriptorResolver
    {
        private readonly AssemblyServiceDescriptorResolver _resolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginServiceDescriptorResolver"/> class.
        /// </summary>
        public PluginServiceDescriptorResolver()
        {
            _resolver = new AssemblyServiceDescriptorResolver();

            var pluginManager = PluginFacility.PluginManager;
            if (pluginManager == null)
                throw new InvalidOperationException("Please add 'PluginFacility' to the DI container before registering components.");

            _resolver.From(pluginManager.Plugins.Select(p => p.Assembly));
            _resolver.WithPrivateTypes(pluginManager.Options.ResolvePrivateTypes);
            _resolver.ClearDefaultFilters();

        }

        /// <summary>
        /// Adds a type filter.
        /// </summary>
        /// <param name="filter">The type filter.</param>
        /// <returns>The <see cref="PluginServiceDescriptorResolver"/>.</returns>
        public PluginServiceDescriptorResolver Filter(Predicate<Type> filter)
        {
            _resolver.Filter(filter);
            return this;
        }

        /// <summary>
        /// Clears the current type filters.
        /// </summary>
        /// <returns>The <see cref="PluginServiceDescriptorResolver"/>.</returns>
        public PluginServiceDescriptorResolver ClearFilters()
        {
            _resolver.ClearFilters();
            return this;
        }

        /// <inheritdoc />
        IEnumerable<ServiceDescriptor> IServiceDescriptorResolver.Resolve(Type serviceType, ServiceLifetime defaultLifetime)
        {
            return ((IServiceDescriptorResolver)_resolver).Resolve(serviceType, defaultLifetime);
        }
    }
}