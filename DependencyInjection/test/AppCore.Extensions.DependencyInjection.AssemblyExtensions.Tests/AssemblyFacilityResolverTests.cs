// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Linq;
using AppCore.Extensions.DependencyInjection.Activator;
using AppCore.Extensions.DependencyInjection.Facilities;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace AppCore.Extensions.DependencyInjection
{
    public class AssemblyFacilityResolverTests
    {
        [Fact]
        public void ResolvesAllFacilities()
        {
            var activator = Substitute.For<IActivator>();
            activator.CreateInstance(Arg.Any<Type>(), Arg.Any<object[]>())
                     .Returns(ci => System.Activator.CreateInstance(ci.ArgAt<Type>(0)));

            AssemblyFacilityResolver resolver = new AssemblyFacilityResolver(activator)
                                                .Add(typeof(AssemblyFacilityResolverTests).Assembly)
                                                .ClearDefaultFilters();

            IEnumerable<Facility> facilities = ((IFacilityResolver) resolver).Resolve();

            facilities.Select(f => f.GetType())
                      .Should()
                      .BeEquivalentTo(new [] { typeof(Facility1), typeof(Facility2) });
        }
    }
}