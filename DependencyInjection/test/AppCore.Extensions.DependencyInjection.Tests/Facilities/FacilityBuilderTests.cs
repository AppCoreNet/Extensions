// Licensed under the MIT License.
// Copyright (c) 2018-2022 the AppCore .NET project.

using System;
using AppCore.Extensions.DependencyInjection.Activator;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Xunit;

namespace AppCore.Extensions.DependencyInjection.Facilities;

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
        _services.Should()
                 .ContainSingle(sd => sd.ServiceType == typeof(FacilityExtensionTestService));
    }

    [Fact]
    public void AddExtensionTypeRegistersServices()
    {
        var builder = new FacilityBuilder<TestFacility>(_services, _activator);
        builder.AddExtension(typeof(TestFacilityExtension));
        _services.Should()
                 .ContainSingle(sd => sd.ServiceType == typeof(FacilityExtensionTestService));
    }

    [Fact]
    public void AddExtensionRegistersServices()
    {
        var builder = new FacilityBuilder<TestFacility>(_services, _activator);
        builder.AddExtension(new TestFacilityExtension());
        _services.Should()
                 .ContainSingle(sd => sd.ServiceType == typeof(FacilityExtensionTestService));
    }

    [Fact]
    public void AddExtensionsFromRegistersServices()
    {
        var resolver = Substitute.For<IFacilityExtensionResolver>();
        resolver.Resolve(Arg.Is(typeof(TestFacility)))
                .Returns(new[] { new TestFacilityExtension() });

        var builder = new FacilityBuilder<TestFacility>(_services, _activator);
        builder.AddExtensionsFrom(r => r.AddResolver(resolver));
        _services.Should()
                 .ContainSingle(sd => sd.ServiceType == typeof(FacilityExtensionTestService));
    }
}