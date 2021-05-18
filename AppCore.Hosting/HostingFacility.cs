// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using AppCore.DependencyInjection;
using AppCore.DependencyInjection.Facilities;
using AppCore.Diagnostics;

namespace AppCore.Hosting
{
    /// <summary>
    /// Represents the facility for the application host.
    /// </summary>
    public class HostingFacility : Facility
    {
        /// <inheritdoc />
        protected override void Build(IComponentRegistry registry)
        {
            base.Build(registry);

            registry.AddLogging();
            registry.TryAdd(ComponentRegistration.Transient<IBackgroundServiceHost, BackgroundServiceHost>());
            registry.TryAdd(ComponentRegistration.Transient<IStartupTaskExecutor, StartupTaskExecutor>());
        }

        /// <summary>
        /// Adds a background service to the container.
        /// </summary>
        /// <param name="backgroundServiceType">The implementation of the <see cref="IBackgroundService"/>.</param>
        /// <returns>The <see cref="HostingFacility"/>.</returns>
        /// <exception cref="ArgumentNullException">Argument <paramref name="backgroundServiceType"/> is <c>null</c>.</exception>
        public HostingFacility WithBackgroundService(Type backgroundServiceType)
        {
            Ensure.Arg.NotNull(backgroundServiceType, nameof(backgroundServiceType));
            Ensure.Arg.OfType<IBackgroundService>(backgroundServiceType, nameof(backgroundServiceType));

            ConfigureRegistry(
                r => r.TryAddEnumerable(
                    ComponentRegistration.Transient(typeof(IBackgroundService), backgroundServiceType)));

            return this;
        }

        /// <summary>
        /// Adds a background service to the container.
        /// </summary>
        /// <typeparam name="T">The implementation of the <see cref="IBackgroundService"/>.</typeparam>
        /// <returns>The <see cref="HostingFacility"/>.</returns>
        public HostingFacility WithBackgroundService<T>()
            where T : IBackgroundService
        {
            return WithBackgroundService(typeof(T));
        }

        /// <summary>
        /// Adds background services to the container.
        /// </summary>
        /// <param name="configure">The configuration delegate.</param>
        /// <returns></returns>
        public HostingFacility WithBackgroundServicesFrom(Action<IComponentRegistrationSources> configure)
        {
            Ensure.Arg.NotNull(configure, nameof(configure));

            ConfigureRegistry(r =>
            {
                var sources = new ComponentRegistrationSources(typeof(IBackgroundService));
                configure(sources);
                r.TryAddEnumerable(sources.GetRegistrations());
            });

            return this;
        }

        /// <summary>
        /// Adds startup tasks to the container.
        /// </summary>
        /// <param name="startupTaskType">The implementation of the <see cref="IStartupTask"/>.</param>
        /// <returns>The <see cref="HostingFacility"/>.</returns>
        /// <exception cref="ArgumentNullException">Argument <paramref name="startupTaskType"/> is <c>null</c>.</exception>
        public HostingFacility WithStartupTask(Type startupTaskType)
        {
            Ensure.Arg.NotNull(startupTaskType, nameof(startupTaskType));
            Ensure.Arg.OfType<IStartupTask>(startupTaskType, nameof(startupTaskType));

            ConfigureRegistry(
                r => r.TryAddEnumerable(
                    ComponentRegistration.Transient(typeof(IStartupTask), startupTaskType)));

            return this;
        }

        /// <summary>
        /// Adds a startup task to the container.
        /// </summary>
        /// <typeparam name="T">The implementation of the <see cref="IStartupTask"/>.</typeparam>
        /// <returns>The <see cref="HostingFacility"/>.</returns>
        public HostingFacility WithStartupTask<T>()
            where T : IStartupTask
        {
            return WithStartupTask(typeof(T));
        }

        /// <summary>
        /// Adds startup tasks to the container.
        /// </summary>
        /// <param name="configure">The configuration delegate.</param>
        /// <returns></returns>
        public HostingFacility WithStartupTasksFrom(Action<IComponentRegistrationSources> configure)
        {
            Ensure.Arg.NotNull(configure, nameof(configure));

            ConfigureRegistry(r =>
            {
                var sources = new ComponentRegistrationSources(typeof(IStartupTask));
                configure(sources);
                r.TryAddEnumerable(sources.GetRegistrations());
            });

            return this;
        }
    }
}
