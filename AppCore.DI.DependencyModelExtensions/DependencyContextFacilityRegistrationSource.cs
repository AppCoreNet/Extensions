// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AppCore.Diagnostics;
using Microsoft.Extensions.DependencyModel;

// ReSharper disable once CheckNamespace
namespace AppCore.DependencyInjection.Facilities
{
    /// <summary>
    /// Builds an <see cref="IEnumerable{T}"/> of <see cref="Facility"/> by scanning assemblies in a
    /// <see cref="DependencyContext"/>.
    /// </summary>
    public class DependencyContextFacilityRegistrationSource : IFacilityRegistrationSource
    {
        private readonly AssemblyFacilityRegistrationSource _source;

        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyContextFacilityRegistrationSource"/> class.
        /// </summary>
        public DependencyContextFacilityRegistrationSource()
        {
            _source = new AssemblyFacilityRegistrationSource();
        }

        /// <summary>
        /// Specifies whether to include private types when scanning for components.
        /// </summary>
        /// <param name="value">A value indicating whether to include private types.</param>
        /// <returns>The <see cref="DependencyContextFacilityRegistrationSource"/>.</returns>
        public DependencyContextFacilityRegistrationSource WithPrivateTypes(bool value = true)
        {
            _source.WithPrivateTypes(value);
            return this;
        }

        /// <summary>
        /// Adds an <see cref="DependencyContext"/> to be scanned for components.
        /// </summary>
        /// <param name="dependencyContext">The <see cref="DependencyContext"/>.</param>
        /// <returns>The <see cref="DependencyContextFacilityRegistrationSource"/>.</returns>
        public DependencyContextFacilityRegistrationSource From(DependencyContext dependencyContext)
        {
            Ensure.Arg.NotNull(dependencyContext, nameof(dependencyContext));

            _source.From(
                dependencyContext.GetDefaultAssemblyNames()
                                 .Select(Assembly.Load));

            return this;
        }

        /// <summary>
        /// Adds a type filter.
        /// </summary>
        /// <param name="filter">The type filter.</param>
        /// <returns>The <see cref="DependencyContextFacilityRegistrationSource"/>.</returns>
        public DependencyContextFacilityRegistrationSource Filter(Predicate<Type> filter)
        {
            _source.Filter(filter);
            return this;
        }

        /// <summary>
        /// Clears the current type filters.
        /// </summary>
        /// <returns>The <see cref="DependencyContextFacilityRegistrationSource"/>.</returns>
        public DependencyContextFacilityRegistrationSource ClearFilters()
        {
            _source.ClearFilters();
            return this;
        }

        /// <summary>
        /// Clears the assembly scanner default type filters.
        /// </summary>
        /// <returns>The <see cref="DependencyContextFacilityRegistrationSource"/>.</returns>
        public DependencyContextFacilityRegistrationSource ClearDefaultFilters()
        {
            _source.ClearDefaultFilters();
            return this;
        }

        /// <inheritdoc />
        IEnumerable<Facility> IFacilityRegistrationSource.GetFacilities()
        {
            return ((IFacilityRegistrationSource)_source).GetFacilities();
        }
    }
}