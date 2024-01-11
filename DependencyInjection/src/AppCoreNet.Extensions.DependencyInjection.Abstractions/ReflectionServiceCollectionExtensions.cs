// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using AppCoreNet.Diagnostics;
using AppCoreNet.Extensions.DependencyInjection.Activator;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AppCoreNet.Extensions.DependencyInjection;

/// <summary>
/// Provides extension methods for the <see cref="IServiceCollection"/> interface.
/// </summary>
public static class ReflectionServiceCollectionExtensions
{
    private static ServiceDescriptorReflectionBuilder CreateBuilder(this IServiceCollection services, Type serviceType)
    {
        Ensure.Arg.NotNull(services);
        Ensure.Arg.NotNull(serviceType);

        services.TryAddTransient<IActivator, ServiceProviderActivator>();
        var serviceProvider = new ServiceCollectionServiceProvider(services);
        var builder = new ServiceDescriptorReflectionBuilder(
            serviceProvider.GetRequiredService<IActivator>(),
            serviceType);

        return builder;
    }

    /// <summary>
    /// Adds services by resolving them from <see cref="IServiceDescriptorResolver"/>'s.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="serviceType">The type of the service.</param>
    /// <param name="configure">The delegate used to configure the resolvers.</param>
    /// <returns>The <see cref="IServiceCollection"/> to allow chaining.</returns>
    /// <exception cref="ArgumentNullException">
    /// Argument <paramref name="services"/>, <paramref name="serviceType"/> or <paramref name="configure"/> is null.
    /// </exception>
    public static IServiceCollection AddFrom(
        this IServiceCollection services,
        Type serviceType,
        Action<IServiceDescriptorReflectionBuilder> configure)
    {
        Ensure.Arg.NotNull(configure);
        ServiceDescriptorReflectionBuilder sources = services.CreateBuilder(serviceType);
        configure(sources);
        return services.Add(sources.Resolve());
    }

    /// <summary>
    /// Adds services by resolving them from <see cref="IServiceDescriptorResolver"/>'s.
    /// </summary>
    /// <typeparam name="TService">The type of the service.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="configure">The delegate used to configure the resolvers.</param>
    /// <returns>The <see cref="IServiceCollection"/> to allow chaining.</returns>
    /// <exception cref="ArgumentNullException">Argument <paramref name="services"/> or <paramref name="configure"/> is null.</exception>
    public static IServiceCollection AddFrom<TService>(
        this IServiceCollection services,
        Action<IServiceDescriptorReflectionBuilder> configure)
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
    /// <returns>The <see cref="IServiceCollection"/> to allow chaining.</returns>
    /// <exception cref="ArgumentNullException">
    /// Argument <paramref name="services"/>, <paramref name="services"/> or <paramref name="configure"/> is null.
    /// </exception>
    public static IServiceCollection TryAddFrom(
        this IServiceCollection services,
        Type serviceType,
        Action<IServiceDescriptorReflectionBuilder> configure)
    {
        Ensure.Arg.NotNull(configure);
        ServiceDescriptorReflectionBuilder sources = services.CreateBuilder(serviceType);
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
    /// <returns>The <see cref="IServiceCollection"/> to allow chaining.</returns>
    /// <exception cref="ArgumentNullException">Argument <paramref name="services"/> or <paramref name="configure"/> is null.</exception>
    public static IServiceCollection TryAddFrom<TService>(
        this IServiceCollection services,
        Action<IServiceDescriptorReflectionBuilder> configure)
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
    /// <returns>The <see cref="IServiceCollection"/> to allow chaining.</returns>
    /// <exception cref="ArgumentNullException">
    /// Argument <paramref name="services"/>, <paramref name="serviceType"/> or <paramref name="configure"/> is null.
    /// </exception>
    public static IServiceCollection TryAddEnumerableFrom(
        this IServiceCollection services,
        Type serviceType,
        Action<IServiceDescriptorReflectionBuilder> configure)
    {
        Ensure.Arg.NotNull(configure);
        ServiceDescriptorReflectionBuilder sources = services.CreateBuilder(serviceType);
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
    /// <returns>The <see cref="IServiceCollection"/> to allow chaining.</returns>
    /// <exception cref="ArgumentNullException">Argument <paramref name="services"/> or <paramref name="configure"/> is null.</exception>
    public static IServiceCollection TryAddEnumerableFrom<TService>(
        this IServiceCollection services,
        Action<IServiceDescriptorReflectionBuilder> configure)
        where TService : class
    {
        return TryAddEnumerableFrom(services, typeof(TService), configure);
    }
}