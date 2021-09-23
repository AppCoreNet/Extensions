// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AppCore.DependencyInjection
{
    public class AssemblyServicesDescriptorResolverTests
    {
        [Fact]
        public void FiltersNonDerivableGenericTypes()
        {
            Type contractType = typeof(IContract);
            AssemblyServiceDescriptorResolver resolver =
                new AssemblyServiceDescriptorResolver()
                    .From(typeof(AssemblyServicesDescriptorResolverTests).Assembly)
                    .ClearDefaultFilters();

            IEnumerable<ServiceDescriptor> serviceDescriptors = ((IServiceDescriptorResolver) resolver).Resolve(contractType, ServiceLifetime.Transient);

            serviceDescriptors
                .Should()
                .BeEquivalentTo(
                    ServiceDescriptor.Describe(
                        contractType,
                        typeof(ContractImpl1),
                        ServiceLifetime.Transient),
                    ServiceDescriptor.Describe(
                        contractType,
                        typeof(ContractImpl2),
                        ServiceLifetime.Transient));
        }

        [Fact]
        public void ClosesOpenGenericInterfaceForClosedGenerics()
        {
            Type contractType = typeof(IContract<>);
            Type closedContractType = typeof(IContract<string>);

            AssemblyServiceDescriptorResolver resolver =
                new AssemblyServiceDescriptorResolver()
                    .From(typeof(AssemblyServicesDescriptorResolverTests).Assembly)
                    .ClearDefaultFilters();

            IEnumerable<ServiceDescriptor> serviceDescriptors = ((IServiceDescriptorResolver) resolver).Resolve(contractType, ServiceLifetime.Transient);

            serviceDescriptors
                .Should()
                .BeEquivalentTo(
                    ServiceDescriptor.Describe(
                        contractType,
                        typeof(ContractImpl1<>),
                        ServiceLifetime.Transient),
                    ServiceDescriptor.Describe(
                        contractType,
                        typeof(ContractImpl2<>),
                        ServiceLifetime.Transient),
                    ServiceDescriptor.Describe(
                        closedContractType,
                        typeof(ContractImpl1String),
                        ServiceLifetime.Transient),
                    ServiceDescriptor.Describe(
                        closedContractType,
                        typeof(ContractImpl2String),
                        ServiceLifetime.Transient));
        }

        [Fact]
        public void UsesLifetimeFromResolver()
        {
            Type contractType = typeof(IContract);
            var lifetime = ServiceLifetime.Scoped;

            AssemblyServiceDescriptorResolver resolver =
                new AssemblyServiceDescriptorResolver()
                    .From(typeof(AssemblyServicesDescriptorResolverTests).Assembly)
                    .ClearDefaultFilters();

            IEnumerable<ServiceDescriptor> registrations = ((IServiceDescriptorResolver) resolver).Resolve(contractType, lifetime);

            registrations.Should()
                         .OnlyContain(cr => cr.Lifetime == lifetime);
        }
    }
}
