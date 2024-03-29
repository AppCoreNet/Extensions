// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Linq;
using AppCoreNet.Extensions.DependencyInjection.Activator;
using AppCoreNet.Extensions.DependencyInjection.Facilities;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace AppCoreNet.Extensions.DependencyInjection;

public class AssemblyResolverTests
{
    private readonly IActivator _activator;

    public AssemblyResolverTests()
    {
        var activator = Substitute.For<IActivator>();
        activator.CreateInstance(Arg.Any<Type>(), Arg.Any<object[]>())
                 .Returns(ci => System.Activator.CreateInstance(ci.ArgAt<Type>(0)));

        _activator = activator;
    }

    [Fact]
    public void ResolvesAllFacilities()
    {
        AssemblyResolver resolver = new AssemblyResolver(_activator)
                                    .Add(typeof(AssemblyResolverTests).Assembly)
                                    .ClearDefaultFilters();

        IEnumerable<IFacility> facilities = ((IFacilityResolver)resolver).Resolve();

        facilities.Select(f => f.GetType())
                  .Should()
                  .BeEquivalentTo(new[] { typeof(Facility1), typeof(Facility2) });
    }

    [Fact]
    public void ResolvesAllFacilityExtensions()
    {
        AssemblyResolver resolver = new AssemblyResolver(_activator)
                                    .Add(typeof(AssemblyResolverTests).Assembly)
                                    .ClearDefaultFilters();

        IEnumerable<IFacilityExtension<IFacility>> facilityExtensions =
            ((IFacilityExtensionResolver)resolver).Resolve(typeof(Facility1));

        facilityExtensions.Should()
                          .AllBeOfType<FacilityExtensionWrapper<Facility1>>();

        facilityExtensions.OfType<FacilityExtensionWrapper<Facility1>>()
                          .Select(e => e.Extension.GetType())
                          .Should()
                          .BeEquivalentTo(new[] { typeof(Facility1Extension1), typeof(Facility1Extension2) });
    }
}