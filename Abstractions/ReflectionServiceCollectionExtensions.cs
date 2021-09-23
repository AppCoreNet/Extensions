// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using AppCore.DependencyInjection;
using AppCore.DependencyInjection.Activator;
using AppCore.DependencyInjection.Facilities;
using AppCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection.Extensions;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Provides extension methods for the <see cref="IServiceCollection"/> interface.
    /// </summary>
    public static class ReflectionServiceCollectionExtensions
    {
        /// <summary>
        /// Adds services by resolving them from <see cref="IServiceDescriptorResolver"/>'s.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="serviceType">The type of the service.</param>
        /// <param name="configure">The delegate used to configure the resolvers.</param>
        /// <returns>The <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddFrom(
            this IServiceCollection services,
            Type serviceType,
            Action<IServiceDescriptorReflectionBuilder> configure)
        {
            Ensure.Arg.NotNull(services, nameof(services));
            Ensure.Arg.NotNull(configure, nameof(configure));

            var sources = new ServiceDescriptorReflectionBuilder(serviceType);
            configure(sources);
            return services.Add(sources.Resolve());
        }

        /// <summary>
        /// Adds services by resolving them from <see cref="IServiceDescriptorResolver"/>'s.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="configure">The delegate used to configure the resolvers.</param>
        /// <returns>The <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddFrom<TService>(
            this IServiceCollection services,
            Action<IServiceDescriptorReflectionBuilder> configure
        )
            where TService : class
        {
            return AddFrom(services, typeof(TService), configure);
        }

        /// <summary>
        /// Adds services by resolving them from <see cref="IServiceDescriptorResolver"/>'s
        /// if the service type has not been already added.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="serviceType">The type of the service.</param>
        /// <param name="configure">The delegate used to configure the resolvers.</param>
        /// <returns>The <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection TryAddFrom(this IServiceCollection services, Type serviceType, Action<IServiceDescriptorReflectionBuilder> configure)
        {
            Ensure.Arg.NotNull(services, nameof(services));
            Ensure.Arg.NotNull(configure, nameof(configure));

            var sources = new ServiceDescriptorReflectionBuilder(serviceType);
            configure(sources);
            services.TryAdd(sources.Resolve());
            return services;
        }

        /// <summary>
        /// Adds services by resolving them from <see cref="IServiceDescriptorResolver"/>'s
        /// if the service type has not been already added.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="configure">The delegate used to configure the resolvers.</param>
        /// <returns>The <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection TryAddFrom<TService>(
            this IServiceCollection services,
            Action<IServiceDescriptorReflectionBuilder> configure
            )
            where TService : class
        {
            return TryAddFrom(services, typeof(TService), configure);
        }

        /// <summary>
        /// Adds services by resolving them from <see cref="IServiceDescriptorResolver"/>'s
        /// if the service type and implementation type has not been already added.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="serviceType">The type of the service.</param>
        /// <param name="configure">The delegate used to configure the resolvers.</param>
        /// <returns>The <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection TryAddEnumerableFrom(
            this IServiceCollection services,
            Type serviceType,
            Action<IServiceDescriptorReflectionBuilder> configure)
        {
            Ensure.Arg.NotNull(services, nameof(services));
            Ensure.Arg.NotNull(configure, nameof(configure));

            var sources = new ServiceDescriptorReflectionBuilder(serviceType);
            configure(sources);
            services.TryAddEnumerable(sources.Resolve());
            return services;
        }

        /// <summary>
        /// Adds services by resolving them from <see cref="IServiceDescriptorResolver"/>'s
        /// if the service type and implementation type has not been already added.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="configure">The delegate used to configure the resolvers.</param>
        /// <returns>The <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection TryAddEnumerableFrom<TService>(
            this IServiceCollection services,
            Action<IServiceDescriptorReflectionBuilder> configure
            )
            where TService : class
        {
            return TryAddEnumerableFrom(services, typeof(TService), configure);
        }

        /// <summary>
        /// Adds facilities using a <see cref="IFacilityReflectionBuilder"/> to the <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="configure">The delegate used to configure the facility resolvers.</param>
        /// <returns>The <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddFacilitiesFrom(
            this IServiceCollection services,
            Action<IFacilityReflectionBuilder> configure)
        {
            Ensure.Arg.NotNull(services, nameof(services));
            Ensure.Arg.NotNull(configure, nameof(configure));

            var serviceProvider = new ServiceCollectionServiceProvider(services);
            var activator = new ServiceProviderActivator(serviceProvider);
            serviceProvider.AddService(typeof(IActivator), activator);

            var facilities = new FacilityReflectionBuilder(activator);
            configure(facilities);

            foreach (Facility facility in facilities.Resolve())
            {
                facility.ConfigureServices(activator, services);
            }

            return services;
        }
    }
}