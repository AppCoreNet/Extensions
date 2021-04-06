// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using AppCore.Diagnostics;

namespace AppCore.DependencyInjection
{
    public class AssemblyRegistrationBuilder
    {
        private Type _contractType;
        private ComponentLifetime _defaultLifetime = ComponentLifetime.Transient;
        private readonly List<Assembly> _assemblies = new List<Assembly>();
        private readonly List<Predicate<Type>> _filters = new List<Predicate<Type>>();
        private bool _clearFilters;

        public AssemblyRegistrationBuilder ForType(Type contractType)
        {
            Ensure.Arg.NotNull(contractType, nameof(contractType));
            _contractType = contractType;
            return this;
        }

        public AssemblyRegistrationBuilder ForType<TContract>()
            where TContract : class
        {
            _contractType = typeof(TContract);
            return this;
        }

        public AssemblyRegistrationBuilder WithAssembly(Assembly assembly)
        {
            Ensure.Arg.NotNull(assembly, nameof(assembly));
            _assemblies.Add(assembly);
            return this;
        }

        public AssemblyRegistrationBuilder WithAssemblies(IEnumerable<Assembly> assemblies)
        {
            Ensure.Arg.NotNull(assemblies, nameof(assemblies));
            _assemblies.AddRange(assemblies);
            return this;
        }

        public AssemblyRegistrationBuilder WithFilter(Predicate<Type> filter)
        {
            _filters.Add(filter);
            return this;
        }

        public AssemblyRegistrationBuilder ClearFilters()
        {
            _clearFilters = true;
            _filters.Clear();
            return this;
        }

        public AssemblyRegistrationBuilder UseDefaultLifetime(ComponentLifetime lifetime)
        {
            _defaultLifetime = lifetime;
            return this;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public IEnumerable<ComponentRegistration> BuildRegistrations()
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

            var scanner = new AssemblyScanner(_contractType, _assemblies);

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