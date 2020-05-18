// Licensed under the MIT License.
// Copyright (c) 2018-2020 the AppCore .NET project.

using System;
using AppCore.DependencyInjection.Builder;
using AppCore.DependencyInjection.Facilities;
using AppCore.Diagnostics;
using AppCore.Hosting;

// ReSharper disable once CheckNamespace
namespace AppCore.DependencyInjection
{
    public static class HostingRegistrationExtensions
    {
        /// <summary>
        /// Adds background services to the container.
        /// </summary>
        /// <param name="builder">The <see cref="IFacilityBuilder{TFacility}"/>.</param>
        /// <param name="registrationBuilder">A delegate to configure the <see cref="IRegistrationBuilder{TContract}"/>.</param>
        /// <returns>The <paramref name="builder"/>.</returns>
        /// <exception cref="ArgumentNullException">Argument <paramref name="builder"/> is <c>null</c>.</exception>
        public static IFacilityBuilder<IHostingFacility> UseBackgroundServices(
            this IFacilityBuilder<IHostingFacility> builder,
            Action<IRegistrationBuilder<IBackgroundService>> registrationBuilder)
        {
            Ensure.Arg.NotNull(builder, nameof(builder));

            return builder.Add(
                new RegistrationFacilityExtension<IHostingFacility, IBackgroundService>(
                    (r,f) =>
                    {
                        r.WithDefaultLifetime(ComponentLifetime.Transient);
                        registrationBuilder(r);
                    }));
        }

        /// <summary>
        /// Adds startup tasks to the container.
        /// </summary>
        /// <param name="builder">The <see cref="IFacilityBuilder{TFacility}"/>.</param>
        /// <param name="registrationBuilder">A delegate to configure the <see cref="IRegistrationBuilder{TContract}"/>.</param>
        /// <returns>The <paramref name="builder"/>.</returns>
        /// <exception cref="ArgumentNullException">Argument <paramref name="builder"/> is <c>null</c>.</exception>
        public static IFacilityBuilder<IHostingFacility> UseStartupTasks(
            this IFacilityBuilder<IHostingFacility> builder,
            Action<IRegistrationBuilder<IStartupTask>> registrationBuilder)
        {
            Ensure.Arg.NotNull(builder, nameof(builder));

            return builder.Add(
                new RegistrationFacilityExtension<IHostingFacility, IStartupTask>(
                    (r, f) =>
                    {
                        r.WithDefaultLifetime(ComponentLifetime.Transient);
                        registrationBuilder(r);
                    }));
        }
    }
}