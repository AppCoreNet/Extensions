// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using AppCore.DependencyInjection.Microsoft.Extensions;
using AppCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace AppCore.DependencyInjection
{
    /// <summary>
    /// Provides extension methods for the <see cref="IComponentRegistry"/> type.
    /// </summary>
    public static class ServiceCollectionComponentRegistryExtensions
    {
        /// <summary>
        /// Adds services to the underlying <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="registry">The <see cref="IComponentRegistry"/>.</param>
        /// <param name="services">The service configuration delegate.</param>
        /// <returns>The <see cref="IComponentRegistry"/>.</returns>
        public static IComponentRegistry AddServices(this IComponentRegistry registry, Action<IServiceCollection> services)
        {
            Ensure.Arg.NotNull(registry, nameof(registry));
            Ensure.Arg.NotNull(services, nameof(services));

            var microsoftRegistry = registry as MicrosoftComponentRegistry;
            if (microsoftRegistry == null)
                throw new InvalidOperationException();

            services(microsoftRegistry.Services);
            return registry;
        }
    }
}