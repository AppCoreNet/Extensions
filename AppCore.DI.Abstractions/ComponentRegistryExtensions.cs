// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
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
        /// Adds the facility with the specified type to the registry.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="Facility"/>.</typeparam>
        /// <param name="registry">The <see cref="IComponentRegistry"/>.</param>
        /// <param name="configure">The delegate to configure the facility.</param>
        /// <returns>The <see cref="IComponentRegistry"/>.</returns>
        public static IComponentRegistry Add<T>(this IComponentRegistry registry, Action<IFacilityBuilder<T>> configure = null)
            where T : Facility
        {
            Ensure.Arg.NotNull(registry, nameof(registry));

            var facility = (T) Facility.Activator.CreateInstance(typeof(T));
            var builder = new FacilityBuilder<T>(registry, facility);
            configure?.Invoke(builder);
            builder.Build();
            return registry;
        }
    }
}