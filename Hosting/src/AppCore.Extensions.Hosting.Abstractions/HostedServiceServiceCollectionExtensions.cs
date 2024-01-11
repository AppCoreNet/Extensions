// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using AppCoreNet.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

// ReSharper disable once CheckNamespace
namespace AppCore.Extensions.DependencyInjection;

/// <summary>
/// Provides extensions methods to register hosted services with a <see cref="IServiceCollection"/>.
/// </summary>
public static class HostedServiceServiceCollectionExtensions
{
    /// <summary>
    /// Adds hosted services to the container.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <param name="configure">The configuration delegate.</param>
    /// <returns>The passed <see cref="IServiceCollection"/> to allow chaining.</returns>
    public static IServiceCollection AddHostedServicesFrom(this IServiceCollection services, Action<IServiceDescriptorReflectionBuilder> configure)
    {
        return services.TryAddEnumerableFrom<IHostedService>(configure);
    }
}