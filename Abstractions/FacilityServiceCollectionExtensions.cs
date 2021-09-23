// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using AppCore.DependencyInjection;
using AppCore.DependencyInjection.Activator;
using AppCore.DependencyInjection.Facilities;
using AppCore.Diagnostics;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Provides extension methods to add facilities to a <see cref="IServiceCollection"/>.
    /// </summary>
    public static class FacilityServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the facility with the specified type to the <see cref="IServiceCollection"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="Facility"/>.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="configure">The delegate to configure the facility.</param>
        /// <returns>The <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddFacility<T>(this IServiceCollection services, Action<T> configure = null)
            where T : Facility
        {
            return AddFacility(services, typeof(T), f => configure?.Invoke((T) f));
        }

        /// <summary>
        /// Adds the facility with the specified type to the <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="facilityType">The type of the <see cref="Facility"/>.</param>
        /// <param name="configure">The delegate to configure the facility.</param>
        /// <returns>The <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddFacility(
            this IServiceCollection services,
            Type facilityType,
            Action<Facility> configure = null)
        {
            Ensure.Arg.NotNull(services, nameof(services));
            Ensure.Arg.NotNull(facilityType, nameof(facilityType));
            Ensure.Arg.OfType(facilityType, typeof(Facility), nameof(facilityType));

            var serviceProvider = new ServiceCollectionServiceProvider(services);
            var activator = new ServiceProviderActivator(serviceProvider);
            serviceProvider.AddService(typeof(IActivator), activator);

            var facility = (Facility)activator.CreateInstance(facilityType);
            configure?.Invoke(facility);
            facility.ConfigureServices(activator, services);
            return services;
        }
    }
}