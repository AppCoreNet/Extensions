// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using System.Runtime.CompilerServices;
using AppCore.DependencyInjection.Facilities;
using AppCore.Diagnostics;

namespace AppCore.DependencyInjection
{
    /// <summary>
    /// Provides extension methods for the <see cref="IComponentRegistry"/> interface.
    /// </summary>
    public static class ComponentRegistryExtensions
    {
        /// <summary>
        /// Adds the specified component registrations to the registry.
        /// </summary>
        /// <param name="registry">The <see cref="IComponentRegistry"/>.</param>
        /// <param name="registration">The <see cref="ComponentRegistration"/> to add.</param>
        /// <returns>The <see cref="IComponentRegistry"/>.</returns>
        public static IComponentRegistry Add(this IComponentRegistry registry, ComponentRegistration registration)
        {
            Ensure.Arg.NotNull(registry, nameof(registry));
            return registry.Add(new[] {registration});
        }

        /// <summary>
        /// Adds component registrations from <see cref="IComponentRegistrationSource"/>'s to the registry.
        /// </summary>
        /// <param name="registry">The <see cref="IComponentRegistry"/>.</param>
        /// <param name="configure">The delegate used to configure the registration sources.</param>
        /// <returns>The <see cref="IComponentRegistry"/>.</returns>
        public static IComponentRegistry AddFrom(this IComponentRegistry registry, Action<IComponentRegistrationSources> configure)
        {
            Ensure.Arg.NotNull(registry, nameof(registry));
            Ensure.Arg.NotNull(configure, nameof(configure));

            var sources = new ComponentRegistrationSources();
            configure(sources);
            return registry.Add(sources.GetRegistrations());
        }

        /// <summary>
        /// Adds the specified component registrations to the registry if the contract of the
        /// component has not been already added.
        /// </summary>
        /// <param name="registry">The <see cref="IComponentRegistry"/>.</param>
        /// <param name="registration">The <see cref="ComponentRegistration"/> to add.</param>
        /// <returns>The <see cref="IComponentRegistry"/>.</returns>
        public static IComponentRegistry TryAdd(this IComponentRegistry registry, ComponentRegistration registration)
        {
            Ensure.Arg.NotNull(registry, nameof(registry));
            return registry.TryAdd(new[] { registration });
        }

        /// <summary>
        /// Adds component registrations from <see cref="IComponentRegistrationSource"/>'s to the registry
        /// if the contract of the component has not been already added.
        /// </summary>
        /// <param name="registry">The <see cref="IComponentRegistry"/>.</param>
        /// <param name="configure">The delegate used to configure the registration sources.</param>
        /// <returns>The <see cref="IComponentRegistry"/>.</returns>
        public static IComponentRegistry TryAddFrom(this IComponentRegistry registry, Action<IComponentRegistrationSources> configure)
        {
            Ensure.Arg.NotNull(registry, nameof(registry));
            Ensure.Arg.NotNull(configure, nameof(configure));

            var sources = new ComponentRegistrationSources();
            configure(sources);
            return registry.TryAdd(sources.GetRegistrations());
        }

        /// <summary>
        /// Adds the specified component registrations to the registry if the contract and implementation
        /// type of the component has not been already added.
        /// </summary>
        /// <param name="registry">The <see cref="IComponentRegistry"/>.</param>
        /// <param name="registration">The <see cref="ComponentRegistration"/> to add.</param>
        /// <returns>The <see cref="IComponentRegistry"/>.</returns>
        public static IComponentRegistry TryAddEnumerable(this IComponentRegistry registry, ComponentRegistration registration)
        {
            Ensure.Arg.NotNull(registry, nameof(registry));
            return registry.TryAddEnumerable(new[] { registration });
        }

        /// <summary>
        /// Adds component registrations from <see cref="IComponentRegistrationSource"/>'s to the registry
        /// if the contract and implementation type of the component has not been already added.
        /// </summary>
        /// <param name="registry">The <see cref="IComponentRegistry"/>.</param>
        /// <param name="configure">The delegate used to configure the registration sources.</param>
        /// <returns>The <see cref="IComponentRegistry"/>.</returns>
        public static IComponentRegistry TryAddEnumerableFrom(this IComponentRegistry registry, Action<IComponentRegistrationSources> configure)
        {
            Ensure.Arg.NotNull(registry, nameof(registry));
            Ensure.Arg.NotNull(configure, nameof(configure));

            var sources = new ComponentRegistrationSources();
            configure(sources);
            return registry.TryAddEnumerable(sources.GetRegistrations());
        }

        /// <summary>
        /// Adds the facility with the specified type to the registry.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="Facility"/>.</typeparam>
        /// <param name="registry">The <see cref="IComponentRegistry"/>.</param>
        /// <param name="configure">The delegate to configure the facility.</param>
        /// <returns>The <see cref="IComponentRegistry"/>.</returns>
        public static IComponentRegistry AddFacility<T>(this IComponentRegistry registry, Action<T> configure = null)
            where T : Facility
        {
            Ensure.Arg.NotNull(registry, nameof(registry));
            var facility = (T) Facility.Activator.CreateInstance(typeof(T));
            return registry.AddFacility(facility, f => configure?.Invoke((T) f));
        }

        /// <summary>
        /// Adds the facility with the specified type to the registry.
        /// </summary>
        /// <param name="registry">The <see cref="IComponentRegistry"/>.</param>
        /// <param name="facility">The <see cref="Facility"/>.</param>
        /// <param name="configure">The delegate to configure the facility.</param>
        /// <returns>The <see cref="IComponentRegistry"/>.</returns>
        public static IComponentRegistry AddFacility(
            this IComponentRegistry registry,
            Facility facility,
            Action<Facility> configure = null)
        {
            Ensure.Arg.NotNull(registry, nameof(registry));
            Ensure.Arg.NotNull(facility, nameof(facility));

            configure?.Invoke(facility);
            facility.Build(registry);
            return registry;
        }

        /// <summary>
        /// Adds facility registrations from <see cref="IFacilityRegistrationSource"/>'s to the registry.
        /// </summary>
        /// <param name="registry">The <see cref="IComponentRegistry"/>.</param>
        /// <param name="configure">The delegate used to configure the registration sources.</param>
        /// <returns>The <see cref="IComponentRegistry"/>.</returns>
        public static IComponentRegistry AddFacilitiesFrom(
            this IComponentRegistry registry,
            Action<IFacilityRegistrationSources> configure)
        {
            Ensure.Arg.NotNull(registry, nameof(registry));
            Ensure.Arg.NotNull(configure, nameof(configure));

            var sources = new FacilityRegistrationSources();
            configure(sources);

            foreach (Facility facility in sources.GetFacilities())
            {
                AddFacility(registry, facility);
            }

            return registry;
        }
    }
}