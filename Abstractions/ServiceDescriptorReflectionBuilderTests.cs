// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using System.Collections.Generic;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Xunit;

namespace AppCore.DependencyInjection
{
    public class ServiceDescriptorReflectionBuilderTests
    {
        [Fact]
        public void AddAssignsServiceTypeToResolver()
        {
            Type serviceType = typeof(IComparable);

            var builder = new ServiceDescriptorReflectionBuilder(serviceType);

            var resolver = Substitute.For<IServiceDescriptorResolver>();
            builder.AddResolver(resolver);

            resolver.Received()
                    .WithServiceType(serviceType);
        }

        [Theory]
        [InlineData(ServiceLifetime.Transient)]
        [InlineData(ServiceLifetime.Scoped)]
        [InlineData(ServiceLifetime.Singleton)]
        public void AddAssignsDefaultLifetimeToResolver(ServiceLifetime lifetime)
        {
            var builder = new ServiceDescriptorReflectionBuilder(null, lifetime);

            var resolver = Substitute.For<IServiceDescriptorResolver>();
            builder.AddResolver(resolver);

            resolver.Received()
                    .WithDefaultLifetime(lifetime);

            resolver.DidNotReceive()
                    .WithServiceType(Arg.Any<Type>());
        }

        [Fact]
        public void ResolveAddsAllServiceDescriptors()
        {
            var builder = new ServiceDescriptorReflectionBuilder();

            ServiceDescriptor serviceDescriptor1 = ServiceDescriptor.Describe(
                typeof(string),
                typeof(string),
                ServiceLifetime.Transient);

            var resolver1 = Substitute.For<IServiceDescriptorResolver>();
            resolver1.Resolve()
                     .Returns(new[] {serviceDescriptor1});

            builder.AddResolver(resolver1);

            ServiceDescriptor serviceDescriptor2 = ServiceDescriptor.Describe(
                typeof(string),
                typeof(string),
                ServiceLifetime.Transient);

            var resolver2 = Substitute.For<IServiceDescriptorResolver>();
            resolver2.Resolve()
                     .Returns(new[] {serviceDescriptor2});

            builder.AddResolver(resolver2);

            IReadOnlyCollection<ServiceDescriptor> serviceDescriptors = builder.Resolve();

            serviceDescriptors.Should()
                              .BeEquivalentTo(serviceDescriptor1, serviceDescriptor2);
        }
    }
}