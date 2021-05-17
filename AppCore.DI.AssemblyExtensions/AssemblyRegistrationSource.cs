// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using AppCore.Diagnostics;

namespace AppCore.DependencyInjection
{
    /// <summary>
    /// Builds an <see cref="IEnumerable{T}"/> of <see cref="ComponentRegistration"/> by scanning assemblies.
    /// </summary>
    public class AssemblyRegistrationSource : IComponentRegistrationSource
    {
        private Type _contractType;
        private ComponentLifetime _defaultLifetime = ComponentLifetime.Transient;
        private readonly List<Assembly> _assemblies = new();
        private readonly List<Predicate<Type>> _filters = new();
        private bool _clearFilters;
        private bool _withPrivateTypes;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyRegistrationSource"/> class.
        /// </summary>
        public AssemblyRegistrationSource()
        {
        }

        /// <summary>
        /// Sets the contract type which is being registered.
        /// </summary>
        /// <param name="contractType">The type of the contract.</param>
        /// <returns>The <see cref="AssemblyRegistrationSource"/>.</returns>
        public AssemblyRegistrationSource WithContract(Type contractType)
        {
            if (_contractType != null)
                throw new InvalidOperationException("The contract type cannot be changed.");

            _contractType = contractType;
            return this;
        }

        /// <summary>
        /// Sets the contract type which is being registered.
        /// </summary>
        /// <typeparam name="TContract">The type of the contract.</typeparam>
        /// <returns>The <see cref="AssemblyRegistrationSource"/>.</returns>
        public AssemblyRegistrationSource WithContract<TContract>()
            where TContract : class
        {
            return WithContract(typeof(TContract));
        }

        /// <inheritdoc />
        void IComponentRegistrationSource.WithContract(Type contractType)
        {
            WithContract(contractType);
        }

        /// <summary>
        /// Specifies the default lifetime for components.
        /// </summary>
        /// <param name="lifetime">The default lifetime.</param>
        /// <returns>The <see cref="AssemblyRegistrationSource"/>.</returns>
        public AssemblyRegistrationSource WithDefaultLifetime(ComponentLifetime lifetime)
        {
            _defaultLifetime = lifetime;
            return this;
        }

        /// <inheritdoc />
        void IComponentRegistrationSource.WithDefaultLifetime(ComponentLifetime lifetime)
        {
            WithDefaultLifetime(lifetime);
        }

        /// <summary>
        /// Specifies whether to include private types when scanning for components.
        /// </summary>
        /// <param name="value">A value indicating whether to include private types.</param>
        /// <returns>The <see cref="AssemblyRegistrationSource"/>.</returns>
        public AssemblyRegistrationSource WithPrivateTypes(bool value = true)
        {
            _withPrivateTypes = value;
            return this;
        }

        /// <summary>
        /// Adds an <see cref="Assembly"/> to be scanned for components.
        /// </summary>
        /// <param name="assembly">The <see cref="Assembly"/>.</param>
        /// <returns>The <see cref="AssemblyRegistrationSource"/>.</returns>
        public AssemblyRegistrationSource From(Assembly assembly)
        {
            Ensure.Arg.NotNull(assembly, nameof(assembly));
            _assemblies.Add(assembly);
            return this;
        }

        /// <summary>
        /// Adds an <see cref="IEnumerable{T}"/> of <see cref="Assembly"/> to be scanned for components.
        /// </summary>
        /// <param name="assemblies">The <see cref="IEnumerable{T}"/> of <see cref="Assembly"/>.</param>
        /// <returns>The <see cref="AssemblyRegistrationSource"/>.</returns>
        public AssemblyRegistrationSource From(IEnumerable<Assembly> assemblies)
        {
            Ensure.Arg.NotNull(assemblies, nameof(assemblies));
            _assemblies.AddRange(assemblies);
            return this;
        }

        /// <summary>
        /// Adds a type filter.
        /// </summary>
        /// <param name="filter">The type filter.</param>
        /// <returns>The <see cref="AssemblyRegistrationSource"/>.</returns>
        public AssemblyRegistrationSource Filter(Predicate<Type> filter)
        {
            Ensure.Arg.NotNull(filter, nameof(filter));
            _filters.Add(filter);
            return this;
        }

        /// <summary>
        /// Clears the current type filters.
        /// </summary>
        /// <returns>The <see cref="AssemblyRegistrationSource"/>.</returns>
        public AssemblyRegistrationSource ClearFilters()
        {
            _filters.Clear();
            return this;
        }

        /// <summary>
        /// Clears the assembly scanner default type filters.
        /// </summary>
        /// <returns>The <see cref="AssemblyRegistrationSource"/>.</returns>
        public AssemblyRegistrationSource ClearDefaultFilters()
        {
            _clearFilters = true;
            return this;
        }

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public IEnumerable<ComponentRegistration> GetRegistrations()
        {
            if (_contractType == null)
            {
                throw new InvalidOperationException(
                    "No contract type has been defined for the assembly component registration.");
            }

            ComponentLifetime GetServiceLifetime(Type implementationType)
            {
                var lifetimeAttribute =
                    implementationType.GetTypeInfo()
                                      .GetCustomAttribute<LifetimeAttribute>();

                return lifetimeAttribute?.Lifetime ?? _defaultLifetime;
            }

            var scanner = new AssemblyScanner(_contractType, _assemblies)
            {
                IncludePrivateTypes = _withPrivateTypes
            };

            if (_clearFilters)
                scanner.Filters.Clear();

            foreach (Predicate<Type> filter in _filters)
                scanner.Filters.Add(filter);
            
            IEnumerable<Type> componentTypes = scanner.ScanAssemblies();
            bool isOpenGenericContractType = scanner.ContractType.IsGenericTypeDefinition;

            foreach (Type componentType in componentTypes)
            {
                bool isOpenGenericComponentType = componentType.GetTypeInfo()
                                                               .IsGenericTypeDefinition;

                // skip non-closed generic component types if contract type is not a open generic type
                if (!isOpenGenericContractType && isOpenGenericComponentType)
                    continue;

                // need to register closed types with closed generic contract type
                Type contractType = scanner.ContractType;
                if (isOpenGenericContractType && !isOpenGenericComponentType)
                    contractType = componentType.GetClosedTypeOf(contractType);

                yield return ComponentRegistration.Create(
                    contractType,
                    componentType,
                    GetServiceLifetime(componentType));
            }
        }
    }
}