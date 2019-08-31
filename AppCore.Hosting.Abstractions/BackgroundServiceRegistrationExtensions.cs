// Licensed under the MIT License.
// Copyright (c) 2018,2019 the AppCore .NET project.

using System;
using AppCore.DependencyInjection.Builder;
using AppCore.DependencyInjection.Facilities;
using AppCore.Diagnostics;
using AppCore.Hosting;

// ReSharper disable once CheckNamespace
namespace AppCore.DependencyInjection
{
    public static class BackgroundServiceRegistrationExtensions
    {
        /// <summary>
        /// Adds background services to the container.
        /// </summary>
        /// <param name="builder">The <see cref="IFacilityBuilder{TFacility}"/>.</param>
        /// <param name="registrationBuilder">A delegate to configure the <see cref="IRegistrationBuilder{TContract}"/>.</param>
        /// <returns>The <paramref name="builder"/>.</returns>
        /// <exception cref="ArgumentNullException">Argument <paramref name="builder"/> is <c>null</c>.</exception>
        public static IFacilityBuilder<IBackgroundServiceFacility> UseServices(
            this IFacilityBuilder<IBackgroundServiceFacility> builder,
            Action<IRegistrationBuilder<IBackgroundService>> registrationBuilder)
        {
            Ensure.Arg.NotNull(builder, nameof(builder));

            return builder.Add(
                new RegistrationFacilityExtension<IBackgroundServiceFacility, IBackgroundService>(
                    (r,f) =>
                    {
                        r.WithDefaultLifetime(ComponentLifetime.Transient);
                        registrationBuilder(r);
                    }));
        }
    }
}