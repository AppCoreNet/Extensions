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
    public class DependencyContextRegistrationBuilder
    {
        private readonly AssemblyRegistrationBuilder _builder;

        internal DependencyContextRegistrationBuilder(Type contractType)
        {
            _builder = AssemblyRegistrationBuilder.ForContract(contractType);
        }

        /// <summary>
        /// Creates a <see cref="DependencyContextRegistrationBuilder"/> for the specified contract.
        /// </summary>
        /// <param name="contractType">The type of the contract.</param>
        /// <returns>The <see cref="DependencyContextRegistrationBuilder"/>.</returns>
        public static DependencyContextRegistrationBuilder ForContract(Type contractType)
        {
            return new DependencyContextRegistrationBuilder(contractType);
        }

        /// <summary>
        /// Creates a <see cref="DependencyContextRegistrationBuilder"/> for the specified contract.
        /// </summary>
        /// <typeparam name="TContract">The type of the contract.</typeparam>
        /// <returns>The <see cref="DependencyContextRegistrationBuilder"/>.</returns>
        public static DependencyContextRegistrationBuilder ForContract<TContract>()
            where TContract : class
        {
            return new DependencyContextRegistrationBuilder(typeof(TContract));
        }

        /// <summary>
        /// Adds an <see cref="DependencyContext"/> to be scanned for components.
        /// </summary>
        /// <param name="dependencyContext">The <see cref="DependencyContext"/>.</param>
        /// <returns>The <see cref="DependencyContextRegistrationBuilder"/>.</returns>
        public DependencyContextRegistrationBuilder From(DependencyContext dependencyContext)
        {
            Ensure.Arg.NotNull(dependencyContext, nameof(dependencyContext));

            _builder.From(
                dependencyContext.GetDefaultAssemblyNames()
                                 .Select(Assembly.Load));

            return this;
        }

        /// <summary>
        /// Specifies the default lifetime for components.
        /// </summary>
        /// <param name="lifetime">The default lifetime.</param>
        /// <returns>The <see cref="DependencyContextRegistrationBuilder"/>.</returns>
        public DependencyContextRegistrationBuilder WithDefaultLifetime(ComponentLifetime lifetime)
        {
            _builder.WithDefaultLifetime(lifetime);
            return this;
        }

        /// <summary>
        /// Adds a type filter.
        /// </summary>
        /// <param name="filter">The type filter.</param>
        /// <returns>The <see cref="DependencyContextRegistrationBuilder"/>.</returns>
        public DependencyContextRegistrationBuilder Filter(Predicate<Type> filter)
        {
            _builder.Filter(filter);
            return this;
        }

        /// <summary>
        /// Clears the current type filters.
        /// </summary>
        /// <returns>The <see cref="DependencyContextRegistrationBuilder"/>.</returns>
        public DependencyContextRegistrationBuilder ClearFilters()
        {
            _builder.ClearFilters();
            return this;
        }

        /// <summary>
        /// Clears the assembly scanner default type filters.
        /// </summary>
        /// <returns>The <see cref="DependencyContextRegistrationBuilder"/>.</returns>
        public DependencyContextRegistrationBuilder ClearDefaultFilters()
        {
            _builder.ClearDefaultFilters();
            return this;
        }

        /// <summary>
        /// Builds the component registrations.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="ComponentRegistration"/>.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public IEnumerable<ComponentRegistration> BuildRegistrations()
        {
            return _builder.BuildRegistrations();
        }
    }
}