// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System.Text;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Xunit;

namespace AppCore.Extensions.DependencyInjection;

public class ReflectionServiceCollectionExtensionsTests
{
    [Fact]
    public void AddFromAddsAllServices()
    {
        ServiceDescriptor service1 = ServiceDescriptor.Describe(
            typeof(string),
            typeof(string),
            ServiceLifetime.Transient);

        ServiceDescriptor service2 = ServiceDescriptor.Describe(
            typeof(string),
            typeof(string),
            ServiceLifetime.Transient);

        var resolver = Substitute.For<IServiceDescriptorResolver>();
        resolver.Resolve(typeof(string), ServiceLifetime.Transient)
                .Returns(new[] { service1, service2 });

        var services = new ServiceCollection();
        services.AddFrom<string>(r => r.AddResolver(resolver));

        services.Should()
                .Contain(new[] { service1, service2 });
    }

    [Fact]
    public void TryAddFromAddsOnlyFirstService()
    {
        ServiceDescriptor service1 = ServiceDescriptor.Describe(
            typeof(Encoding),
            typeof(UTF8Encoding),
            ServiceLifetime.Transient);

        ServiceDescriptor service2 = ServiceDescriptor.Describe(
            typeof(Encoding),
            typeof(ASCIIEncoding),
            ServiceLifetime.Transient);

        var resolver = Substitute.For<IServiceDescriptorResolver>();
        resolver.Resolve(typeof(Encoding), ServiceLifetime.Transient)
                .Returns(new[] { service1, service2 });

        var services = new ServiceCollection();
        services.TryAddFrom<Encoding>(r => r.AddResolver(resolver));

        services.Should()
                .Contain(new[] { service1 });
    }

    [Fact]
    public void TryAddEnumerableFromAddsAllServices()
    {
        ServiceDescriptor service1 = ServiceDescriptor.Describe(
            typeof(Encoding),
            typeof(UTF8Encoding),
            ServiceLifetime.Transient);

        ServiceDescriptor service2 = ServiceDescriptor.Describe(
            typeof(Encoding),
            typeof(ASCIIEncoding),
            ServiceLifetime.Transient);

        var resolver = Substitute.For<IServiceDescriptorResolver>();
        resolver.Resolve(typeof(Encoding), ServiceLifetime.Transient)
                .Returns(new[] { service1, service2 });

        var services = new ServiceCollection();
        services.TryAddEnumerableFrom<Encoding>(r => r.AddResolver(resolver));

        services.Should()
                .Contain(new[] { service1, service2 });
    }

    [Fact]
    public void TryAddEnumerableFromAddsOnlyFirstService()
    {
        ServiceDescriptor service1 = ServiceDescriptor.Describe(
            typeof(Encoding),
            typeof(UTF8Encoding),
            ServiceLifetime.Transient);

        ServiceDescriptor service2 = ServiceDescriptor.Describe(
            typeof(Encoding),
            typeof(UTF8Encoding),
            ServiceLifetime.Transient);

        var resolver = Substitute.For<IServiceDescriptorResolver>();
        resolver.Resolve(typeof(Encoding), ServiceLifetime.Transient)
                .Returns(new[] { service1, service2 });

        var services = new ServiceCollection();
        services.TryAddEnumerableFrom<Encoding>(r => r.AddResolver(resolver));

        services.Should()
                .Contain(new[] { service1 });
    }
}