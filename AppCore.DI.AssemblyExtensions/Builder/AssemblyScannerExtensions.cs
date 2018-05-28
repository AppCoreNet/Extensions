// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Reflection;

namespace AppCore.DependencyInjection.Builder
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

            bool isOpenGenericService = registrationInfo.ContractType.GetTypeInfo()
                                                    .IsGenericTypeDefinition;

            foreach (Type componentType in componentTypes)
            {
                Type contractType = registrationInfo.ContractType;

                // need to register closed types with closed generic contract type
                if (isOpenGenericService && !componentType.GetTypeInfo()
                                                          .IsGenericTypeDefinition)
                {
                    contractType = componentType.GetClosedTypeOf(contractType);
                }

                yield return ComponentRegistration.Create(
                    contractType,
                    componentType,
                    GetServiceLifetime(componentType),
                    registrationInfo.Flags);
            }
        }
    }
}