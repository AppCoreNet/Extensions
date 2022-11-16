// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using AppCore.Diagnostics;
using AppCore.Extensions.DependencyInjection;
using AppCore.Extensions.DependencyInjection.Activator;
using AppCore.Extensions.DependencyInjection.Facilities;
using Microsoft.Extensions.DependencyInjection.Extensions;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

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
    /// <returns>The <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddFacility<T>(this IServiceCollection services, Action<T>? configure = null)
        where T : IFacility
    {
        return AddFacility(services, typeof(T), f => configure?.Invoke((T) f));
    }

    /// <summary>
    /// Adds the facility with the specified type to the <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="facilityType">The type of the <see cref="IFacility"/>.</param>
    /// <param name="configure">The delegate to configure the facility.</param>
    /// <returns>The <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddFacility(
        this IServiceCollection services,
        Type facilityType,
        Action<IFacility>? configure = null)
    {
        Ensure.Arg.NotNull(services);
        Ensure.Arg.NotNull(facilityType);
        Ensure.Arg.OfType(facilityType, typeof(IFacility));

        services.TryAddTransient<IActivator, ServiceProviderActivator>();

        var serviceProvider = new ServiceCollectionServiceProvider(services);
        var activator = serviceProvider.GetRequiredService<IActivator>();

        var facility = (IFacility)activator.CreateInstance(facilityType);
        configure?.Invoke(facility);
        facility.ConfigureServices(services);

        return services;
    }
}