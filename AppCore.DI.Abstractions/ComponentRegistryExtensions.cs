// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System;
using AppCore.DependencyInjection.Builder;
using AppCore.Diagnostics;

namespace AppCore.DependencyInjection
{
    public static class ComponentRegistryExtensions
    {
        public static void Register(this IComponentRegistry registry, ComponentRegistration registration)
        {
            Ensure.Arg.NotNull(registry, nameof(registry));
            registry.RegisterCallback(() => new[] { registration });
        }

        public static IRegistrationBuilder Register(this IComponentRegistry registry, Type contractType)
        {
            Ensure.Arg.NotNull(registry, nameof(registry));
            Ensure.Arg.NotNull(contractType, nameof(contractType));
            return new RegistrationBuilder(registry, contractType);
        }

        public static IRegistrationBuilder<TContract> Register<TContract>(this IComponentRegistry registry)
        {
            Ensure.Arg.NotNull(registry, nameof(registry));
            return new RegistrationBuilder<TContract>(registry);
        }

        /// <summary>
        /// Registers a facility with the dependency injection container.
        /// </summary>
        /// <param name="registry">The <see cref="IComponentRegistry"/> where to register components.</param>
        /// <param name="facility">The <see cref="IFacility"/> which is registered.</param>
        /// <returns>The <see cref="IFacilityBuilder"/>.</returns>
        public static IFacilityBuilder RegisterFacility(
            this IComponentRegistry registry,
            IFacility facility)
        {
            Ensure.Arg.NotNull(registry, nameof(registry));
            Ensure.Arg.NotNull(facility, nameof(facility));

            var builder = new FacilityBuilder(facility);
            var facilityRegistry = new FacilityComponentRegistry();

            registry.RegisterCallback(
                () =>
                {
                    builder.RegisterComponents(facilityRegistry);
                    return facilityRegistry.GetComponentRegistrations();
                });

            return builder;
        }

        /// <summary>
        /// Registers a facility with the dependency injection container.
        /// </summary>
        /// <typeparam name="TFacility">The type of the facility.</typeparam>
        /// <param name="registry">The <see cref="IComponentRegistry"/> where to register components.</param>
        /// <returns>The <see cref="IFacilityBuilder{TFacility}"/>.</returns>
        public static IFacilityBuilder<TFacility> RegisterFacility<TFacility>(this IComponentRegistry registry)
            where TFacility : IFacility, new()
        {
            Ensure.Arg.NotNull(registry, nameof(registry));

            var builder = new FacilityBuilder<TFacility>(new TFacility());
            var facilityRegistry = new FacilityComponentRegistry();

            registry.RegisterCallback(() =>
            {
                builder.RegisterComponents(facilityRegistry);
                return facilityRegistry.GetComponentRegistrations();
            });

            return builder;
        }
    }
}