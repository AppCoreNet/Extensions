// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System.Linq;
using AppCore.DependencyInjection.Facilities;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using Xunit;

namespace AppCore.DependencyInjection
{
    public class FacilityRegistrationTests
    {
        [Fact]
        public void FacilityRegistersComponents()
        {
            var registry = new TestComponentRegistry();
            registry.RegisterFacility<TestFacility>();

            registry.GetRegistrations()
                    .Should()
                    .BeEquivalentTo(
                        new[]
                        {
                            ComponentRegistration.Create(
                                typeof(ITestFacilityService),
                                typeof(TestFacilityService),
                                ComponentLifetime.Transient)
                        });
        }

        [Fact]
        public void FacilityRegistersExtensionComponents()
        {
            var registry = new TestComponentRegistry();

            registry.RegisterFacility<TestFacility>()
                    .Add<TestFacilityExtension>();

            registry.GetRegistrations()
                    .Should()
                    .BeEquivalentTo(
                        new[]
                        {
                            ComponentRegistration.Create(
                                typeof(ITestFacilityService),
                                typeof(TestFacilityService),
                                ComponentLifetime.Transient),

                            ComponentRegistration.Create(
                                typeof(ITestFacilityService),
                                typeof(TestFacilityExtensionService),
                                ComponentLifetime.Transient)
                        });


        }

        [Fact]
        public void FacilityInstanceIsPassedToExtension()
        {
            var registry = new TestComponentRegistry();
            var facility = new TestFacility();
            var extension = Substitute.For<IFacilityExtension<TestFacility>>();

            facility.Extensions.Add(extension);

            registry.RegisterFacility(facility);
            registry.GetRegistrations();

            ((IFacilityExtension) extension).Received(Quantity.Exactly(1))
                                            .RegisterComponents(Arg.Any<IComponentRegistry>(), facility);
        }
    }
}
