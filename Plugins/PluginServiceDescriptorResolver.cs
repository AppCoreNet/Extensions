// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Linq;
using AppCore.Hosting.Plugins;
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
        /// Sets the contract type which is being registered.
        /// </summary>
        /// <param name="contractType">The type of the contract.</param>
        /// <returns>The <see cref="PluginServiceDescriptorResolver"/>.</returns>
        public PluginServiceDescriptorResolver WithServiceType(Type contractType)
        {
            _resolver.WithServiceType(contractType);
            return this;
        }

        /// <summary>
        /// Sets the contract type which is being registered.
        /// </summary>
        /// <typeparam name="TContract">The type of the contract.</typeparam>
        /// <returns>The <see cref="PluginServiceDescriptorResolver"/>.</returns>
        public PluginServiceDescriptorResolver WithServiceType<TContract>()
            where TContract : class
        {
            _resolver.WithServiceType<TContract>();
            return this;
        }

        void IServiceDescriptorResolver.WithServiceType(Type contractType)
        {
            WithServiceType(contractType);
        }

        /// <summary>
        /// Specifies the default lifetime for components.
        /// </summary>
        /// <param name="lifetime">The default lifetime.</param>
        /// <returns>The <see cref="PluginServiceDescriptorResolver"/>.</returns>
        public PluginServiceDescriptorResolver WithDefaultLifetime(ServiceLifetime lifetime)
        {
            _resolver.WithDefaultLifetime(lifetime);
            return this;
        }

        /// <inheritdoc />
        void IServiceDescriptorResolver.WithDefaultLifetime(ServiceLifetime lifetime)
        {
            WithDefaultLifetime(lifetime);
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
        IEnumerable<ServiceDescriptor> IServiceDescriptorResolver.Resolve()
        {
            return ((IServiceDescriptorResolver)_resolver).Resolve();
        }
    }
}