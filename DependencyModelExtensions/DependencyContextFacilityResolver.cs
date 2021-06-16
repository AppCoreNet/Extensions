// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AppCore.DependencyInjection.Activator;
using AppCore.Diagnostics;
using Microsoft.Extensions.DependencyModel;

// ReSharper disable once CheckNamespace
namespace AppCore.DependencyInjection.Facilities
{
    /// <summary>
    /// Builds an <see cref="IEnumerable{T}"/> of <see cref="Facility"/> by scanning assemblies in a
    /// <see cref="DependencyContext"/>.
    /// </summary>
    public class DependencyContextFacilityResolver : IFacilityResolver
    {
        private readonly AssemblyFacilityResolver _resolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyContextFacilityResolver"/> class.
        /// </summary>
        public DependencyContextFacilityResolver(IActivator activator)
        {
            _resolver = new AssemblyFacilityResolver(activator);
        }

        /// <summary>
        /// Specifies whether to include private types when scanning for components.
        /// </summary>
        /// <param name="value">A value indicating whether to include private types.</param>
        /// <returns>The <see cref="DependencyContextFacilityResolver"/>.</returns>
        public DependencyContextFacilityResolver WithPrivateTypes(bool value = true)
        {
            _resolver.WithPrivateTypes(value);
            return this;
        }

        /// <summary>
        /// Adds an <see cref="DependencyContext"/> to be scanned for components.
        /// </summary>
        /// <param name="dependencyContext">The <see cref="DependencyContext"/>.</param>
        /// <returns>The <see cref="DependencyContextFacilityResolver"/>.</returns>
        public DependencyContextFacilityResolver From(DependencyContext dependencyContext)
        {
            Ensure.Arg.NotNull(dependencyContext, nameof(dependencyContext));

            _resolver.From(
                dependencyContext.GetDefaultAssemblyNames()
                                 .Select(Assembly.Load));

            return this;
        }

        /// <summary>
        /// Adds a type filter.
        /// </summary>
        /// <param name="filter">The type filter.</param>
        /// <returns>The <see cref="DependencyContextFacilityResolver"/>.</returns>
        public DependencyContextFacilityResolver Filter(Predicate<Type> filter)
        {
            _resolver.Filter(filter);
            return this;
        }

        /// <summary>
        /// Clears the current type filters.
        /// </summary>
        /// <returns>The <see cref="DependencyContextFacilityResolver"/>.</returns>
        public DependencyContextFacilityResolver ClearFilters()
        {
            _resolver.ClearFilters();
            return this;
        }

        /// <summary>
        /// Clears the assembly scanner default type filters.
        /// </summary>
        /// <returns>The <see cref="DependencyContextFacilityResolver"/>.</returns>
        public DependencyContextFacilityResolver ClearDefaultFilters()
        {
            _resolver.ClearDefaultFilters();
            return this;
        }

        /// <inheritdoc />
        IEnumerable<Facility> IFacilityResolver.Resolve()
        {
            return ((IFacilityResolver)_resolver).Resolve();
        }
    }
}