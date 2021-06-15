// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using System.Collections.Generic;
using AppCore.DependencyInjection.Facilities;
using FluentAssertions;
using Xunit;

namespace AppCore.DependencyInjection
{
    public class AssemblyFacilityResolverTests
    {
        [Fact]
        public void ResolvesAllFacilities()
        {
            AssemblyFacilityResolver resolver = new AssemblyFacilityResolver()
                                                .From(typeof(AssemblyFacilityResolverTests).Assembly)
                                                .ClearDefaultFilters();

            IEnumerable<Type> facilities = ((IFacilityResolver) resolver).Resolve();

            facilities.Should()
                      .BeEquivalentTo(typeof(Facility1), typeof(Facility2));
        }
    }
}