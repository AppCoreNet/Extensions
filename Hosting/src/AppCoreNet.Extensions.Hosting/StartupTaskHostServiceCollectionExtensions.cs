// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using AppCoreNet.Diagnostics;
using AppCoreNet.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

// ReSharper disable once CheckNamespace
namespace AppCoreNet.Extensions.DependencyInjection;

/// <summary>
/// Provides extensions methods to register startup tasks with a <see cref="IServiceCollection"/>.
/// </summary>
public static class StartupTaskHostServiceCollectionExtensions
{
    /// <summary>
    /// Adds execution of startup tasks.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <returns>The passed <see cref="IServiceCollection"/> to allow chaining.</returns>
    /// <exception cref="ArgumentNullException">Argument <paramref name="services"/> is <c>null</c>.</exception>
    public static IServiceCollection AddStartupTaskHost(this IServiceCollection services)
    {
        Ensure.Arg.NotNull(services);
        services.TryAddEnumerable(ServiceDescriptor.Transient<IHostedService, StartupTaskHostedService>());
        return services;
    }
}