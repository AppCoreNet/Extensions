// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System;
using AppCore.DependencyInjection;
using AppCore.DependencyInjection.Facilities;
using AppCore.DependencyInjection.Microsoft.Extensions;
using AppCore.Diagnostics;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Provides extensions methods to register facilities with a <see cref="IServiceCollection"/>.
    /// </summary>
    public static class FacilityServiceCollectionExtensions
    {
        /// <summary>
        /// Register the specified <typeparamref name="TFacility"/> with the service collection.
        /// </summary>
        /// <typeparam name="TFacility">The type of the <see cref="IFacility"/> to register.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/> where to register the facility.</param>
        /// <param name="configure">Optional delegate to configure the facility.</param>
        /// <returns>The <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddFacility<TFacility>(
            this IServiceCollection services,
            Action<IFacilityBuilder<TFacility>> configure = null)
            where TFacility : IFacility, new()
        {
            Ensure.Arg.NotNull(services, nameof(services));

            var registry = new MicrosoftComponentRegistry();
            IFacilityBuilder<TFacility> facilityBuilder = registry.RegisterFacility<TFacility>();
            configure?.Invoke(facilityBuilder);
            registry.RegisterComponents(services);
            return services;
        }
    }
}