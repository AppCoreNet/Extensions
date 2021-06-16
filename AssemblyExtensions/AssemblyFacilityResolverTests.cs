// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Linq;
using AppCore.DependencyInjection.Activator;
using AppCore.DependencyInjection.Facilities;
using FluentAssertions;
using NSubstitute;
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

            var activator = Substitute.For<IActivator>();
            activator.CreateInstance(Arg.Any<Type>(), Arg.Any<object[]>())
                     .Returns(ci => System.Activator.CreateInstance(ci.ArgAt<Type>(0)));


            IEnumerable<Facility> facilities = ((IFacilityResolver) resolver).Resolve(activator);

            facilities.Select(f => f.GetType())
                      .Should()
                      .BeEquivalentTo(typeof(Facility1), typeof(Facility2));
        }
    }
}