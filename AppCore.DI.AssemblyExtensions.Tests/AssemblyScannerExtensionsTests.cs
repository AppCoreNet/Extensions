// Licensed under the MIT License.
// Copyright (c) 2018,2019 the AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Reflection;
using AppCore.DependencyInjection.Builder;
using FluentAssertions;
using Xunit;

namespace AppCore.DependencyInjection
{
    public class AssemblyScannerExtensionsTests
    {
        [Fact]
        public void FiltersNonDerivableGenericTypes()
        {
            Type contractType = typeof(IContract);
            var scanner = new AssemblyScanner(contractType);

            scanner.Filters.Clear();
            scanner.Assemblies.Add(typeof(AssemblyScannerTests).GetTypeInfo().Assembly);

            IEnumerable<ComponentRegistration> registrations =
                scanner.CreateRegistrations(new AssemblyRegistrationInfo(contractType));

            registrations
                .Should()
                .BeEquivalentTo(
                    ComponentRegistration.Create(
                        contractType,
                        typeof(ContractImpl1),
                        ComponentLifetime.Transient),
                    ComponentRegistration.Create(
                        contractType,
                        typeof(ContractImpl2),
                        ComponentLifetime.Transient));
        }

        [Fact]
        public void ClosesOpenGenericInterfaceForClosedGenerics()
        {
            Type contractType = typeof(IContract<>);
            Type closedContractType = typeof(IContract<string>);
            var scanner = new AssemblyScanner(contractType);

            scanner.Filters.Clear();
            scanner.Assemblies.Add(typeof(AssemblyScannerTests).GetTypeInfo().Assembly);

            IEnumerable<ComponentRegistration> registrations =
                scanner.CreateRegistrations(new AssemblyRegistrationInfo(contractType));

            registrations
                .Should()
                .BeEquivalentTo(
                    ComponentRegistration.Create(
                        contractType,
                        typeof(ContractImpl1<>),
                        ComponentLifetime.Transient),
                    ComponentRegistration.Create(
                        contractType,
                        typeof(ContractImpl2<>),
                        ComponentLifetime.Transient),
                    ComponentRegistration.Create(
                        closedContractType,
                        typeof(ContractImpl1String),
                        ComponentLifetime.Transient),
                    ComponentRegistration.Create(
                        closedContractType,
                        typeof(ContractImpl2String),
                        ComponentLifetime.Transient));
        }

        [Fact]
        public void UsesLifetimeFromRegistrationInfo()
        {
            Type contractType = typeof(IContract);
            var lifetime = ComponentLifetime.Scoped;
            var scanner = new AssemblyScanner(contractType);

            scanner.Filters.Clear();
            scanner.Assemblies.Add(typeof(AssemblyScannerTests).GetTypeInfo().Assembly);

            IEnumerable<ComponentRegistration> registrations =
                scanner.CreateRegistrations(new AssemblyRegistrationInfo(contractType) { Lifetime = lifetime });

            registrations.Should()
                         .OnlyContain(cr => cr.Lifetime == lifetime);
        }
    }
}
