// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Linq;
using AppCore.Hosting.Plugins;

// ReSharper disable once CheckNamespace
namespace AppCore.DependencyInjection
{
    /// <summary>
    /// Builds an <see cref="IEnumerable{T}"/> of <see cref="ComponentRegistration"/> by scanning plugin assemblies. 
    /// </summary>
    public class PluginComponentRegistrationSource : IComponentRegistrationSource
    {
        private readonly AssemblyComponentRegistrationSource _source;

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginComponentRegistrationSource"/> class.
        /// </summary>
        public PluginComponentRegistrationSource()
        {
            _source = new AssemblyComponentRegistrationSource();

            var pluginManager = PluginManager.Instance;
            if (pluginManager == null)
                throw new InvalidOperationException("Please add 'PluginHostingFacility' to the DI container before registering components.");

            _source.From(pluginManager.Plugins.Select(p => p.Assembly));
            _source.WithPrivateTypes(pluginManager.Options.ResolvePrivateTypes);
            _source.ClearDefaultFilters();

        }

        /// <summary>
        /// Sets the contract type which is being registered.
        /// </summary>
        /// <param name="contractType">The type of the contract.</param>
        /// <returns>The <see cref="PluginComponentRegistrationSource"/>.</returns>
        public PluginComponentRegistrationSource WithContract(Type contractType)
        {
            _source.WithContract(contractType);
            return this;
        }

        /// <summary>
        /// Sets the contract type which is being registered.
        /// </summary>
        /// <typeparam name="TContract">The type of the contract.</typeparam>
        /// <returns>The <see cref="PluginComponentRegistrationSource"/>.</returns>
        public PluginComponentRegistrationSource WithContract<TContract>()
            where TContract : class
        {
            _source.WithContract<TContract>();
            return this;
        }

        void IComponentRegistrationSource.WithContract(Type contractType)
        {
            WithContract(contractType);
        }

        /// <summary>
        /// Specifies the default lifetime for components.
        /// </summary>
        /// <param name="lifetime">The default lifetime.</param>
        /// <returns>The <see cref="PluginComponentRegistrationSource"/>.</returns>
        public PluginComponentRegistrationSource WithDefaultLifetime(ComponentLifetime lifetime)
        {
            _source.WithDefaultLifetime(lifetime);
            return this;
        }

        /// <inheritdoc />
        void IComponentRegistrationSource.WithDefaultLifetime(ComponentLifetime lifetime)
        {
            WithDefaultLifetime(lifetime);
        }

        /// <summary>
        /// Adds a type filter.
        /// </summary>
        /// <param name="filter">The type filter.</param>
        /// <returns>The <see cref="PluginComponentRegistrationSource"/>.</returns>
        public PluginComponentRegistrationSource Filter(Predicate<Type> filter)
        {
            _source.Filter(filter);
            return this;
        }

        /// <summary>
        /// Clears the current type filters.
        /// </summary>
        /// <returns>The <see cref="PluginComponentRegistrationSource"/>.</returns>
        public PluginComponentRegistrationSource ClearFilters()
        {
            _source.ClearFilters();
            return this;
        }

        /// <inheritdoc />
        IEnumerable<ComponentRegistration> IComponentRegistrationSource.GetRegistrations()
        {
            return ((IComponentRegistrationSource)_source).GetRegistrations();
        }
    }
}