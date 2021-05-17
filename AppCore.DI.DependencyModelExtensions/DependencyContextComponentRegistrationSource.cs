// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AppCore.Diagnostics;
using Microsoft.Extensions.DependencyModel;

namespace AppCore.DependencyInjection
{
    /// <summary>
    /// Builds an <see cref="IEnumerable{T}"/> of <see cref="ComponentRegistration"/> by scanning assemblies in a
    /// <see cref="DependencyContext"/>.
    /// </summary>
    public class DependencyContextComponentRegistrationSource : IComponentRegistrationSource
    {
        private readonly AssemblyComponentRegistrationSource _source;

        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyContextComponentRegistrationSource"/> class.
        /// </summary>
        public DependencyContextComponentRegistrationSource()
        {
            _source = new AssemblyComponentRegistrationSource();
        }

        /// <summary>
        /// Sets the contract type which is being registered.
        /// </summary>
        /// <param name="contractType">The type of the contract.</param>
        /// <returns>The <see cref="DependencyContextComponentRegistrationSource"/>.</returns>
        public DependencyContextComponentRegistrationSource WithContract(Type contractType)
        {
            _source.WithContract(contractType);
            return this;
        }

        /// <summary>
        /// Sets the contract type which is being registered.
        /// </summary>
        /// <typeparam name="TContract">The type of the contract.</typeparam>
        /// <returns>The <see cref="DependencyContextComponentRegistrationSource"/>.</returns>
        public DependencyContextComponentRegistrationSource WithContract<TContract>()
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
        /// Specifies whether to include private types when scanning for components.
        /// </summary>
        /// <param name="value">A value indicating whether to include private types.</param>
        /// <returns>The <see cref="DependencyContextComponentRegistrationSource"/>.</returns>
        public DependencyContextComponentRegistrationSource WithPrivateTypes(bool value = true)
        {
            _source.WithPrivateTypes(value);
            return this;
        }

        /// <summary>
        /// Adds an <see cref="DependencyContext"/> to be scanned for components.
        /// </summary>
        /// <param name="dependencyContext">The <see cref="DependencyContext"/>.</param>
        /// <returns>The <see cref="DependencyContextComponentRegistrationSource"/>.</returns>
        public DependencyContextComponentRegistrationSource From(DependencyContext dependencyContext)
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
        /// <returns>The <see cref="DependencyContextComponentRegistrationSource"/>.</returns>
        public DependencyContextComponentRegistrationSource WithDefaultLifetime(ComponentLifetime lifetime)
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
        /// <returns>The <see cref="DependencyContextComponentRegistrationSource"/>.</returns>
        public DependencyContextComponentRegistrationSource Filter(Predicate<Type> filter)
        {
            _source.Filter(filter);
            return this;
        }

        /// <summary>
        /// Clears the current type filters.
        /// </summary>
        /// <returns>The <see cref="DependencyContextComponentRegistrationSource"/>.</returns>
        public DependencyContextComponentRegistrationSource ClearFilters()
        {
            _source.ClearFilters();
            return this;
        }

        /// <summary>
        /// Clears the assembly scanner default type filters.
        /// </summary>
        /// <returns>The <see cref="DependencyContextComponentRegistrationSource"/>.</returns>
        public DependencyContextComponentRegistrationSource ClearDefaultFilters()
        {
            _source.ClearDefaultFilters();
            return this;
        }

        /// <inheritdoc />
        IEnumerable<ComponentRegistration> IComponentRegistrationSource.GetRegistrations()
        {
            return ((IComponentRegistrationSource) _source).GetRegistrations();
        }
    }
}