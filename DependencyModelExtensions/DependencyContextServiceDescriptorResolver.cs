// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AppCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;

namespace AppCore.DependencyInjection
{
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
        /// Sets the contract type which is being registered.
        /// </summary>
        /// <param name="serviceType">The type of the contract.</param>
        /// <returns>The <see cref="DependencyContextServiceDescriptorResolver"/>.</returns>
        public DependencyContextServiceDescriptorResolver WithServiceType(Type serviceType)
        {
            _source.WithServiceType(serviceType);
            return this;
        }

        /// <summary>
        /// Sets the contract type which is being registered.
        /// </summary>
        /// <typeparam name="TContract">The type of the contract.</typeparam>
        /// <returns>The <see cref="DependencyContextServiceDescriptorResolver"/>.</returns>
        public DependencyContextServiceDescriptorResolver WithServiceType<TContract>()
            where TContract : class
        {
            _source.WithServiceType<TContract>();
            return this;
        }

        void IServiceDescriptorResolver.WithServiceType(Type serviceType)
        {
            WithServiceType(serviceType);
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
        public DependencyContextServiceDescriptorResolver From(DependencyContext dependencyContext)
        {
            Ensure.Arg.NotNull(dependencyContext, nameof(dependencyContext));

            _source.From(
                dependencyContext.GetDefaultAssemblyNames()
                                 .Select(Assembly.Load));

            return this;
        }

        /// <summary>
        /// Specifies the default lifetime for components.
        /// </summary>
        /// <param name="lifetime">The default lifetime.</param>
        /// <returns>The <see cref="DependencyContextServiceDescriptorResolver"/>.</returns>
        public DependencyContextServiceDescriptorResolver WithDefaultLifetime(ServiceLifetime lifetime)
        {
            _source.WithDefaultLifetime(lifetime);
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
        IEnumerable<ServiceDescriptor> IServiceDescriptorResolver.Resolve()
        {
            return ((IServiceDescriptorResolver) _source).Resolve();
        }
    }
}