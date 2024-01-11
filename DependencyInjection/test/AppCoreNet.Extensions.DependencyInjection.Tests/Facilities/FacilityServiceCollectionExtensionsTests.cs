// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Xunit;

namespace AppCoreNet.Extensions.DependencyInjection.Facilities;

public class FacilityServiceCollectionExtensionsTests
{
    private readonly ServiceCollection _services;

    public FacilityServiceCollectionExtensionsTests()
    {
        _services = new ServiceCollection();
    }

    [Fact]
    public void AddFacilityGenericTypeRegistersServices()
    {
        _services.AddFacility<TestFacility>();

        _services.Should()
                 .ContainSingle(sd => sd.ServiceType == typeof(FacilityTestService));
    }

    [Fact]
    public void AddFacilityTypeRegistersServices()
    {
        _services.AddFacility(typeof(TestFacility));

        _services.Should()
                 .ContainSingle(sd => sd.ServiceType == typeof(FacilityTestService));
    }

    [Fact]
    public void AddFacilityTypeWithContractRegistersServices()
    {
        _services.AddFacility(typeof(TestFacility));

        _services.Should()
                 .ContainSingle(sd => sd.ServiceType == typeof(FacilityTestService));
    }

    [Fact]
    public void AddFacilitiesFromRegistersServices()
    {
        var resolver = Substitute.For<IFacilityResolver>();
        resolver.Resolve()
                .Returns(new[] { new TestFacility() });

        _services.AddFacilitiesFrom(r => r.AddResolver(resolver));

        _services.Should()
                 .ContainSingle(sd => sd.ServiceType == typeof(FacilityTestService));
    }
}