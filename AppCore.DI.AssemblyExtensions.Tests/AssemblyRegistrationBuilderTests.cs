// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace AppCore.DependencyInjection
{
    public class AssemblyRegistrationBuilderTests
    {
        [Fact]
        public void FiltersNonDerivableGenericTypes()
        {
            Type contractType = typeof(IContract);
            AssemblyRegistrationBuilder scanner =
                new AssemblyRegistrationBuilder()
                    .ForType(contractType)
                    .WithAssembly(typeof(AssemblyRegistrationBuilderTests).Assembly)
                    .ClearFilters()
                    .UseDefaultLifetime(ComponentLifetime.Transient);

            IEnumerable<ComponentRegistration> registrations = scanner.BuildRegistrations();

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

            AssemblyRegistrationBuilder scanner =
                new AssemblyRegistrationBuilder()
                    .ForType(contractType)
                    .WithAssembly(typeof(AssemblyRegistrationBuilderTests).Assembly)
                    .ClearFilters()
                    .UseDefaultLifetime(ComponentLifetime.Transient);

            IEnumerable<ComponentRegistration> registrations = scanner.BuildRegistrations();

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

            AssemblyRegistrationBuilder scanner =
                new AssemblyRegistrationBuilder()
                    .ForType(contractType)
                    .WithAssembly(typeof(AssemblyRegistrationBuilderTests).Assembly)
                    .ClearFilters()
                    .UseDefaultLifetime(lifetime);

            IEnumerable<ComponentRegistration> registrations = scanner.BuildRegistrations();

            registrations.Should()
                         .OnlyContain(cr => cr.Lifetime == lifetime);
        }
    }
}
