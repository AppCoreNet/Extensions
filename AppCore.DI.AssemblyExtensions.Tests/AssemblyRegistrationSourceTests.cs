// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace AppCore.DependencyInjection
{
    public class AssemblyRegistrationSourceTests
    {
        [Fact]
        public void FiltersNonDerivableGenericTypes()
        {
            Type contractType = typeof(IContract);
            AssemblyComponentRegistrationSource scanner =
                new AssemblyComponentRegistrationSource()
                    .WithContract(contractType)
                    .From(typeof(AssemblyRegistrationSourceTests).Assembly)
                    .ClearDefaultFilters()
                    .WithDefaultLifetime(ComponentLifetime.Transient);

            IEnumerable<ComponentRegistration> registrations = ((IComponentRegistrationSource) scanner).GetRegistrations();

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

            AssemblyComponentRegistrationSource scanner =
                new AssemblyComponentRegistrationSource()
                    .WithContract(contractType)
                    .From(typeof(AssemblyRegistrationSourceTests).Assembly)
                    .ClearDefaultFilters()
                    .WithDefaultLifetime(ComponentLifetime.Transient);

            IEnumerable<ComponentRegistration> registrations = ((IComponentRegistrationSource) scanner).GetRegistrations();

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

            AssemblyComponentRegistrationSource scanner =
                new AssemblyComponentRegistrationSource()
                    .WithContract(contractType)
                    .From(typeof(AssemblyRegistrationSourceTests).Assembly)
                    .ClearDefaultFilters()
                    .WithDefaultLifetime(lifetime);

            IEnumerable<ComponentRegistration> registrations = ((IComponentRegistrationSource) scanner).GetRegistrations();

            registrations.Should()
                         .OnlyContain(cr => cr.Lifetime == lifetime);
        }
    }
}
