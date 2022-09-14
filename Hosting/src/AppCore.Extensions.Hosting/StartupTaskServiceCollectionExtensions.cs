// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using AppCore.Diagnostics;
using AppCore.Extensions.DependencyInjection;
using AppCore.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Provides extensions methods to register startup tasks with a <see cref="IServiceCollection"/>.
/// </summary>
public static class StartupTaskServiceCollectionExtensions
{
    /// <summary>
    /// Adds execution of startup tasks.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <returns>The passed <see cref="IServiceCollection"/> to allow chaining.</returns>
    /// <exception cref="ArgumentNullException">Argument <paramref name="services"/> is <c>null</c>.</exception>
    public static IServiceCollection AddStartupTasks(this IServiceCollection services)
    {
        Ensure.Arg.NotNull(services);
        services.TryAddEnumerable(ServiceDescriptor.Transient<IHostedService, StartupTaskHostedService>());
        return services;
    }

    /// <summary>
    /// Adds startup tasks to the container.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="startupTaskType">The implementation of the <see cref="IStartupTask"/>.</param>
    /// <returns>The passed <see cref="IServiceCollection"/> to allow chaining.</returns>
    /// <exception cref="ArgumentNullException">Argument <paramref name="startupTaskType"/> is <c>null</c>.</exception>
    public static IServiceCollection AddStartupTask(this IServiceCollection services, Type startupTaskType)
    {
        Ensure.Arg.NotNull(startupTaskType);
        Ensure.Arg.OfType<IStartupTask>(startupTaskType);

        services.AddStartupTasks()
                .TryAddEnumerable(ServiceDescriptor.Transient(typeof(IStartupTask), startupTaskType));

        return services;
    }

    /// <summary>
    /// Adds a startup task to the container.
    /// </summary>
    /// <typeparam name="T">The implementation of the <see cref="IStartupTask"/>.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <returns>The passed <see cref="IServiceCollection"/> to allow chaining.</returns>
    public static IServiceCollection AddStartupTask<T>(this IServiceCollection services)
        where T : IStartupTask
    {
        return AddStartupTask(services, typeof(T));
    }

    /// <summary>
    /// Adds startup tasks to the container.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="configure">The configuration delegate.</param>
    /// <returns>The passed <see cref="IServiceCollection"/> to allow chaining.</returns>
    public static IServiceCollection AddStartupTasksFrom(this IServiceCollection services, Action<IServiceDescriptorReflectionBuilder> configure)
    {
        return services.AddStartupTasks()
                       .TryAddEnumerableFrom<IStartupTask>(configure);
    }
}