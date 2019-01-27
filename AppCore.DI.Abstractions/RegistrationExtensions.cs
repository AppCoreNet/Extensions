// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System;
using AppCore.DependencyInjection.Builder;
using AppCore.DependencyInjection.Facilities;
using AppCore.Diagnostics;

namespace AppCore.DependencyInjection
{
    /// <summary>
    /// Provides extension methods for registering components with the <see cref="IComponentRegistry"/>.
    /// </summary>
    public static partial class RegistrationExtensions
    {
        /// <summary>
        /// Registers a <see cref="ComponentRegistration"/> with the component registry.
        /// </summary>
        /// <param name="registry">The <see cref="IComponentRegistry"/> where the component is registered.</param>
        /// <param name="registration">The <see cref="ComponentRegistration"/> which is registered.</param>
        public static void Register(this IComponentRegistry registry, ComponentRegistration registration)
        {
            Ensure.Arg.NotNull(registry, nameof(registry));
            registry.RegisterCallback(() => new[] { registration });
        }

        /// <summary>
        /// Registers a component for the specified <paramref name="contractType"/>.
        /// </summary>
        /// <param name="registry">The <see cref="IComponentRegistry"/> where the component is registered.</param>
        /// <param name="contractType">The type of the contract.</param>
        /// <returns>The <see cref="IRegistrationBuilder"/> which is used to register the component.</returns>
        public static IRegistrationBuilder Register(this IComponentRegistry registry, Type contractType)
        {
            Ensure.Arg.NotNull(registry, nameof(registry));
            Ensure.Arg.NotNull(contractType, nameof(contractType));
            return new RegistrationBuilder(registry, contractType);
        }

        /// <summary>
        /// Registers a component for the specified <typeparamref name="TContract"/>.
        /// </summary>
        /// <typeparam name="TContract">The type of the contract.</typeparam>
        /// <param name="registry">The <see cref="IComponentRegistry"/> where the component is registered.</param>
        /// <returns>The <see cref="IRegistrationBuilder{TContract}"/> which is used to register the component.</returns>
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