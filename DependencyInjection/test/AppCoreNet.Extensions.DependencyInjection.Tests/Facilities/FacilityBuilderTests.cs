// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Linq;
using AppCoreNet.Extensions.DependencyInjection.Activator;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Xunit;

namespace AppCoreNet.Extensions.DependencyInjection.Facilities;

public class FacilityBuilderTests
{
    private readonly IActivator _activator;
    private readonly ServiceCollection _services;

    public FacilityBuilderTests()
    {
        var activator = Substitute.For<IActivator>();
        activator.CreateInstance(Arg.Any<Type>(), Arg.Any<object[]>())
                 .Returns(ci => System.Activator.CreateInstance(ci.ArgAt<Type>(0), ci.ArgAt<object[]>(1)));

        _activator = activator;
        _services = new ServiceCollection();
    }

    [Fact]
    public void AddExtensionGenericTypeRegistersServices()
    {
        var builder = new FacilityBuilder<TestFacility>(_services, _activator);
        builder.AddExtension<TestFacilityExtension>();
        builder.AddExtension<TestFacilityContractExtension>();
        _services.Select(sd => sd.ServiceType)
                 .Should()
                 .BeEquivalentTo(new[] { typeof(FacilityExtensionTestService), typeof(FacilityExtensionTestService) });
    }

    [Fact]
    public void AddExtensionForContractGenericTypeRegistersServices()
    {
        var builder = new FacilityBuilder<ITestFacility>(_services, _activator);
        builder.AddExtension<TestFacilityContractExtension>();
        _services.Should()
                 .Contain(sd => sd.ServiceType == typeof(FacilityExtensionTestService));
    }

    [Fact]
    public void AddExtensionTypeRegistersServices()
    {
        var builder = new FacilityBuilder<TestFacility>(_services, _activator);
        builder.AddExtension(typeof(TestFacilityExtension));
        builder.AddExtension(typeof(TestFacilityContractExtension));
        _services.Select(sd => sd.ServiceType)
                 .Should()
                 .BeEquivalentTo(new[] { typeof(FacilityExtensionTestService), typeof(FacilityExtensionTestService) });
    }

    [Fact]
    public void AddExtensionRegistersServices()
    {
        var builder = new FacilityBuilder<TestFacility>(_services, _activator);
        builder.AddExtension(new TestFacilityExtension());
        builder.AddExtension(new TestFacilityContractExtension());

        _services.Select(sd => sd.ServiceType)
                 .Should()
                 .BeEquivalentTo(new[] { typeof(FacilityExtensionTestService), typeof(FacilityExtensionTestService) });
    }

    [Fact]
    public void AddExtensionsFromInvokesResolver()
    {
        var resolver = Substitute.For<IFacilityExtensionResolver>();
        resolver.Resolve(Arg.Any<Type>())
                .Returns(Array.Empty<IFacilityExtension>());

        var builder = new FacilityBuilder<TestFacility>(_services, _activator);
        builder.AddExtensionsFrom(r => r.AddResolver(resolver));

        resolver.Received()
                .Resolve(typeof(TestFacility));
    }
}