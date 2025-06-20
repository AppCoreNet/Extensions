// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AppCoreNet.Extensions.DependencyInjection.Activator;
using AppCoreNet.Extensions.DependencyInjection.Facilities;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace AppCoreNet.Extensions.DependencyInjection;

[RequiresUnreferencedCode("This test requires types that may be trimmed by the linker.")]
public class AssemblyResolverTests
{
    private readonly IActivator _activator;

    public AssemblyResolverTests()
    {
        _activator = DefaultActivator.Instance;
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

        IFacilityExtension[] facilityExtensions =
            ((IFacilityExtensionResolver)resolver).Resolve(typeof(Facility1))
                                                  .ToArray();

        facilityExtensions[0]
            .Should()
            .BeOfType<Facility1Extension1>();

        facilityExtensions[1]
            .Should()
            .BeOfType<Facility1Extension2>();
    }
}