// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using AppCore.Diagnostics;
using AppCore.Hosting;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
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
            Ensure.Arg.NotNull(services, nameof(services));
            services.TryAddEnumerable(ServiceDescriptor.Transient<IHostedService, StartupTaskHostedService>());
            return services;
        }
    }
}
