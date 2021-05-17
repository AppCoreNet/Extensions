// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public class DependencyContextRegistrationSource : IComponentRegistrationSource
    {
        private readonly AssemblyRegistrationSource _source;

        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyContextRegistrationSource"/> class.
        /// </summary>
        public DependencyContextRegistrationSource()
        {
            _source = new AssemblyRegistrationSource();
        }

        /// <summary>
        /// Sets the contract type which is being registered.
        /// </summary>
        /// <param name="contractType">The type of the contract.</param>
        /// <returns>The <see cref="DependencyContextRegistrationSource"/>.</returns>
        public DependencyContextRegistrationSource WithContract(Type contractType)
        {
            _source.WithContract(contractType);
            return this;
        }

        /// <summary>
        /// Sets the contract type which is being registered.
        /// </summary>
        /// <typeparam name="TContract">The type of the contract.</typeparam>
        /// <returns>The <see cref="DependencyContextRegistrationSource"/>.</returns>
        public DependencyContextRegistrationSource WithContract<TContract>()
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
        /// <returns>The <see cref="DependencyContextRegistrationSource"/>.</returns>
        public DependencyContextRegistrationSource WithPrivateTypes(bool value = true)
        {
            _source.WithPrivateTypes(value);
            return this;
        }

        /// <summary>
        /// Adds an <see cref="DependencyContext"/> to be scanned for components.
        /// </summary>
        /// <param name="dependencyContext">The <see cref="DependencyContext"/>.</param>
        /// <returns>The <see cref="DependencyContextRegistrationSource"/>.</returns>
        public DependencyContextRegistrationSource From(DependencyContext dependencyContext)
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
        /// <returns>The <see cref="DependencyContextRegistrationSource"/>.</returns>
        public DependencyContextRegistrationSource WithDefaultLifetime(ComponentLifetime lifetime)
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
        /// <returns>The <see cref="DependencyContextRegistrationSource"/>.</returns>
        public DependencyContextRegistrationSource Filter(Predicate<Type> filter)
        {
            _source.Filter(filter);
            return this;
        }

        /// <summary>
        /// Clears the current type filters.
        /// </summary>
        /// <returns>The <see cref="DependencyContextRegistrationSource"/>.</returns>
        public DependencyContextRegistrationSource ClearFilters()
        {
            _source.ClearFilters();
            return this;
        }

        /// <summary>
        /// Clears the assembly scanner default type filters.
        /// </summary>
        /// <returns>The <see cref="DependencyContextRegistrationSource"/>.</returns>
        public DependencyContextRegistrationSource ClearDefaultFilters()
        {
            _source.ClearDefaultFilters();
            return this;
        }

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public IEnumerable<ComponentRegistration> BuildRegistrations()
        {
            return ((IComponentRegistrationSource) _source).BuildRegistrations();
        }
    }
}