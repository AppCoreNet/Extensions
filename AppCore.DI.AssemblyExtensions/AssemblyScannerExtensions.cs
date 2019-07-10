// Licensed under the MIT License.
// Copyright (c) 2018,2019 the AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Reflection;
using AppCore.DependencyInjection.Builder;

namespace AppCore.DependencyInjection
{
    internal static class AssemblyScannerExtensions
    {
        public static IEnumerable<ComponentRegistration> CreateRegistrations(
            this AssemblyScanner scanner,
            AssemblyRegistrationInfo registrationInfo)
        {
            IEnumerable<Type> componentTypes = scanner.ScanAssemblies();

            ComponentLifetime GetServiceLifetime(Type implementationType)
            {
                var lifetimeAttribute =
                    implementationType.GetTypeInfo()
                                      .GetCustomAttribute<LifetimeAttribute>();

                return lifetimeAttribute?.Lifetime ?? registrationInfo.Lifetime;
            }

            bool isOpenGenericContractType = registrationInfo.ContractType.GetTypeInfo()
                                                             .IsGenericTypeDefinition;

            foreach (Type componentType in componentTypes)
            {
                bool isOpenGenericComponentType = componentType.GetTypeInfo()
                                                               .IsGenericTypeDefinition;

                // skip non-closed generic component types if contract type is not a open generic type
                if (!isOpenGenericContractType && isOpenGenericComponentType)
                    continue;

                // need to register closed types with closed generic contract type
                Type contractType = registrationInfo.ContractType;
                if (isOpenGenericContractType && !isOpenGenericComponentType)
                    contractType = componentType.GetClosedTypeOf(contractType);

                yield return ComponentRegistration.Create(
                    contractType,
                    componentType,
                    GetServiceLifetime(componentType),
                    registrationInfo.Flags);
            }
        }
    }
}