// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System.Text;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Xunit;

namespace AppCore.DependencyInjection
{
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
            resolver.Resolve()
                    .Returns(new[] {service1, service2});

            var services = new ServiceCollection();
            services.AddFrom(r => r.AddResolver(resolver));

            services.Should()
                    .BeEquivalentTo(service1, service2);
        }

        [Fact]
        public void TryAddFromAddsAllServices()
        {
            ServiceDescriptor service1 = ServiceDescriptor.Describe(
                typeof(string),
                typeof(string),
                ServiceLifetime.Transient);

            ServiceDescriptor service2 = ServiceDescriptor.Describe(
                typeof(char),
                typeof(char),
                ServiceLifetime.Transient);

            var resolver = Substitute.For<IServiceDescriptorResolver>();
            resolver.Resolve()
                    .Returns(new[] { service1, service2 });

            var services = new ServiceCollection();
            services.TryAddFrom(r => r.AddResolver(resolver));

            services.Should()
                    .BeEquivalentTo(service1, service2);
        }

        [Fact]
        public void TryAddFromAddsOnlyFirstService()
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
            resolver.Resolve()
                    .Returns(new[] { service1, service2 });

            var services = new ServiceCollection();
            services.TryAddFrom(r => r.AddResolver(resolver));

            services.Should()
                    .BeEquivalentTo(service1);
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
            resolver.Resolve()
                    .Returns(new[] { service1, service2 });

            var services = new ServiceCollection();
            services.TryAddEnumerableFrom(r => r.AddResolver(resolver));

            services.Should()
                    .BeEquivalentTo(service1, service2);
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
            resolver.Resolve()
                    .Returns(new[] { service1, service2 });

            var services = new ServiceCollection();
            services.TryAddEnumerableFrom(r => r.AddResolver(resolver));

            services.Should()
                    .BeEquivalentTo(service1);
        }

        [Fact]
        public void AddFacilitiesFromAddsAllFacilities()
        {

        }
    }
}