// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Reflection;
using AppCore.Diagnostics;

// ReSharper disable once CheckNamespace
namespace AppCore.DependencyInjection.Facilities
{
    /// <summary>
    /// Builds an <see cref="IEnumerable{T}"/> of <see cref="ComponentRegistration"/> by scanning assemblies.
    /// </summary>
    public class AssemblyFacilityRegistrationSource : IFacilityRegistrationSource
    {
        private readonly List<Assembly> _assemblies = new();
        private readonly List<Predicate<Type>> _filters = new();
        private bool _clearFilters;
        private bool _withPrivateTypes;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyFacilityRegistrationSource"/> class.
        /// </summary>
        public AssemblyFacilityRegistrationSource()
        {
        }

        /// <summary>
        /// Specifies whether to include private types when scanning for components.
        /// </summary>
        /// <param name="value">A value indicating whether to include private types.</param>
        /// <returns>The <see cref="AssemblyFacilityRegistrationSource"/>.</returns>
        public AssemblyFacilityRegistrationSource WithPrivateTypes(bool value = true)
        {
            _withPrivateTypes = value;
            return this;
        }

        /// <summary>
        /// Adds an <see cref="Assembly"/> to be scanned for components.
        /// </summary>
        /// <param name="assembly">The <see cref="Assembly"/>.</param>
        /// <returns>The <see cref="AssemblyFacilityRegistrationSource"/>.</returns>
        public AssemblyFacilityRegistrationSource From(Assembly assembly)
        {
            Ensure.Arg.NotNull(assembly, nameof(assembly));
            _assemblies.Add(assembly);
            return this;
        }

        /// <summary>
        /// Adds an <see cref="IEnumerable{T}"/> of <see cref="Assembly"/> to be scanned for components.
        /// </summary>
        /// <param name="assemblies">The <see cref="IEnumerable{T}"/> of <see cref="Assembly"/>.</param>
        /// <returns>The <see cref="AssemblyFacilityRegistrationSource"/>.</returns>
        public AssemblyFacilityRegistrationSource From(IEnumerable<Assembly> assemblies)
        {
            Ensure.Arg.NotNull(assemblies, nameof(assemblies));
            _assemblies.AddRange(assemblies);
            return this;
        }

        /// <summary>
        /// Adds a type filter.
        /// </summary>
        /// <param name="filter">The type filter.</param>
        /// <returns>The <see cref="AssemblyFacilityRegistrationSource"/>.</returns>
        public AssemblyFacilityRegistrationSource Filter(Predicate<Type> filter)
        {
            Ensure.Arg.NotNull(filter, nameof(filter));
            _filters.Add(filter);
            return this;
        }

        /// <summary>
        /// Clears the current type filters.
        /// </summary>
        /// <returns>The <see cref="AssemblyFacilityRegistrationSource"/>.</returns>
        public AssemblyFacilityRegistrationSource ClearFilters()
        {
            _filters.Clear();
            return this;
        }

        /// <summary>
        /// Clears the assembly scanner default type filters.
        /// </summary>
        /// <returns>The <see cref="AssemblyFacilityRegistrationSource"/>.</returns>
        public AssemblyFacilityRegistrationSource ClearDefaultFilters()
        {
            _clearFilters = true;
            return this;
        }

        /// <inheritdoc />
        IEnumerable<Facility> IFacilityRegistrationSource.GetFacilities()
        {
            var scanner = new AssemblyScanner(typeof(Facility), _assemblies)
            {
                IncludePrivateTypes = _withPrivateTypes
            };

            if (_clearFilters)
                scanner.Filters.Clear();

            foreach (Predicate<Type> filter in _filters)
                scanner.Filters.Add(filter);
            
            IEnumerable<Type> facilityTypes = scanner.ScanAssemblies();

            foreach (Type facilityType in facilityTypes)
            {
                yield return (Facility) Facility.Activator.CreateInstance(facilityType);
            }
        }
    }
}