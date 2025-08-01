// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Diagnostics.CodeAnalysis;
using AppCoreNet.Diagnostics;
using AppCoreNet.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

// ReSharper disable once CheckNamespace
namespace AppCoreNet.Extensions.DependencyInjection;

/// <summary>
/// Provides extensions methods to register startup tasks with a <see cref="IServiceCollection"/>.
/// </summary>
public static class StartupTaskServiceCollectionExtensions
{
    /// <summary>
    /// Adds startup tasks to the container.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="startupTaskType">The implementation of the <see cref="IStartupTask"/>.</param>
    /// <returns>The passed <see cref="IServiceCollection"/> to allow chaining.</returns>
    /// <exception cref="ArgumentNullException">Argument <paramref name="startupTaskType"/> is <c>null</c>.</exception>
    public static IServiceCollection AddStartupTask(
        this IServiceCollection services,
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type startupTaskType)
    {
        Ensure.Arg.NotNull(startupTaskType);
        Ensure.Arg.OfType<IStartupTask>(startupTaskType);

        services.TryAddEnumerable(ServiceDescriptor.Transient(typeof(IStartupTask), startupTaskType));
        return services;
    }

    /// <summary>
    /// Adds a startup task to the container.
    /// </summary>
    /// <typeparam name="T">The implementation of the <see cref="IStartupTask"/>.</typeparam>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <returns>The passed <see cref="IServiceCollection"/> to allow chaining.</returns>
    public static IServiceCollection AddStartupTask<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(
        this IServiceCollection services)
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
    public static IServiceCollection AddStartupTasksFrom(
        this IServiceCollection services,
        Action<IServiceDescriptorReflectionBuilder> configure)
    {
        return services.TryAddEnumerableFrom<IStartupTask>(configure);
    }
}