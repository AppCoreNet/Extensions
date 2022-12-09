// Licensed under the MIT License.
// Copyright (c) 2018-2022 the AppCore .NET project.

using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using Xunit;

namespace AppCore.Extensions.DependencyInjection.Facilities;

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