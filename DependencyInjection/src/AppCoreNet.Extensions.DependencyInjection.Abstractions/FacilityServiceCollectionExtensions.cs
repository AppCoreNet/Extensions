// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Collections.Generic;
using AppCoreNet.Diagnostics;
using AppCoreNet.Extensions.DependencyInjection.Activator;
using AppCoreNet.Extensions.DependencyInjection.Facilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AppCoreNet.Extensions.DependencyInjection;

/// <summary>
/// Provides extension methods to add facilities to a <see cref="IServiceCollection"/>.
/// </summary>
public static class FacilityServiceCollectionExtensions
{
    /// <summary>
    /// Adds the facility with the specified type to the <see cref="IServiceCollection"/>.
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="IFacility"/>.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="configure">The delegate to configure the facility.</param>
    /// <returns>The <see cref="IServiceCollection"/> to allow chaining.</returns>
    /// <exception cref="ArgumentNullException">Argument <paramref name="services"/> is null.</exception>
    public static IServiceCollection AddFacility<T>(this IServiceCollection services, Action<FacilityBuilder<T>>? configure = null)
        where T : IFacility
    {
        Ensure.Arg.NotNull(services);

        services.TryAddTransient<IActivator, ServiceProviderActivator>();

        var serviceProvider = new ServiceCollectionServiceProvider(services);
        var activator = serviceProvider.GetRequiredService<IActivator>();

        var facility = activator.CreateInstance<T>() !;
        if (configure != null)
        {
            var builder = new FacilityBuilder<T>(services, activator);
            configure(builder);
        }

        facility.ConfigureServices(services);
        return services;
    }

    /// <summary>
    /// Adds the facility with the specified type to the <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="facilityType">The type of the <see cref="IFacility"/>.</param>
    /// <param name="configure">The delegate to configure the facility.</param>
    /// <returns>The <see cref="IServiceCollection"/> to allow chaining.</returns>
    /// <exception cref="ArgumentNullException">Argument <paramref name="services"/> or <paramref name="facilityType"/> is null.</exception>
    /// <exception cref="ArgumentException">Argument <paramref name="facilityType"/> is not of type <see cref="IFacility"/>.</exception>
    public static IServiceCollection AddFacility(
        this IServiceCollection services,
        Type facilityType,
        Action<FacilityBuilder>? configure = null)
    {
        Ensure.Arg.NotNull(services);
        Ensure.Arg.NotNull(facilityType);
        Ensure.Arg.OfType(facilityType, typeof(IFacility));

        services.TryAddTransient<IActivator, ServiceProviderActivator>();

        var serviceProvider = new ServiceCollectionServiceProvider(services);
        var activator = serviceProvider.GetRequiredService<IActivator>();

        var facility = (IFacility)activator.CreateInstance(facilityType) !;
        if (configure != null)
        {
            var builder = new FacilityBuilder(services, activator, facilityType);
            configure(builder);
        }

        facility.ConfigureServices(services);
        return services;
    }

    /// <summary>
    /// Adds facilities using a <see cref="IFacilityReflectionBuilder"/> to the <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="configure">The delegate used to configure the facility resolvers.</param>
    /// <returns>The <see cref="IServiceCollection"/> to allow chaining.</returns>
    /// <exception cref="ArgumentNullException">Argument <paramref name="services"/> or <paramref name="configure"/> is null.</exception>
    public static IServiceCollection AddFacilitiesFrom(
        this IServiceCollection services,
        Action<IFacilityReflectionBuilder> configure)
    {
        Ensure.Arg.NotNull(services);
        Ensure.Arg.NotNull(configure);

        services.TryAddTransient<IActivator, ServiceProviderActivator>();
        var serviceProvider = new ServiceCollectionServiceProvider(services);
        var activator = serviceProvider.GetRequiredService<IActivator>();
        var builder = new FacilityReflectionBuilder(activator);

        configure(builder);

        foreach ((IFacility facility,
                     IReadOnlyCollection<IFacilityExtension<IFacility>> facilityExtensions) in builder
                     .Resolve())
        {
            facility.ConfigureServices(services);

            foreach (IFacilityExtension<IFacility> facilityExtension in facilityExtensions)
            {
                facilityExtension.ConfigureServices(services);
            }
        }

        return services;
    }
}