// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System.Collections.Generic;
using AppCoreNet.Extensions.DependencyInjection.Activator;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Xunit;

namespace AppCoreNet.Extensions.DependencyInjection;

public class ServiceDescriptorReflectionBuilderTests
{
    [Fact]
    public void ResolveAddsAllServiceDescriptors()
    {
        var builder = new ServiceDescriptorReflectionBuilder(Substitute.For<IActivator>(), typeof(string));

        ServiceDescriptor serviceDescriptor1 = ServiceDescriptor.Describe(
            typeof(string),
            typeof(string),
            ServiceLifetime.Transient);

        var resolver1 = Substitute.For<IServiceDescriptorResolver>();
        resolver1.Resolve(typeof(string), ServiceLifetime.Transient)
                 .Returns(new[] { serviceDescriptor1 });

        builder.AddResolver(resolver1);

        ServiceDescriptor serviceDescriptor2 = ServiceDescriptor.Describe(
            typeof(string),
            typeof(string),
            ServiceLifetime.Transient);

        var resolver2 = Substitute.For<IServiceDescriptorResolver>();
        resolver2.Resolve(typeof(string), ServiceLifetime.Transient)
                 .Returns(new[] { serviceDescriptor2 });

        builder.AddResolver(resolver2);

        IReadOnlyCollection<ServiceDescriptor> serviceDescriptors = builder.Resolve();

        serviceDescriptors.Should()
                          .BeEquivalentTo(new[] { serviceDescriptor1, serviceDescriptor2 });
    }
}