// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using AppCore.DependencyInjection;
using AppCore.DependencyInjection.Facilities;
using AppCore.DependencyInjection.Microsoft.Extensions;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Provides extension methods for <see cref="IServiceCollection"/>.
    /// </summary>
    public static class FacilityServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the facility of the specified type to the registry.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="Facility"/>.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/>.</param>
        /// <param name="configure">The delegate to configure the facility.</param>
        /// <returns>The <see cref="IComponentRegistry"/>.</returns>
        public static IServiceCollection AddFacility<T>(this IServiceCollection services, Action<T> configure = null)
            where T : Facility
        {
            var registry = new MicrosoftComponentRegistry(services);
            registry.AddFacility(configure);
            return services;
        }
    }
}
