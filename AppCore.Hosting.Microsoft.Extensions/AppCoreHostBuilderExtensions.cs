// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using AppCore.DependencyInjection;
using AppCore.DependencyInjection.Activator;
using AppCore.DependencyInjection.Facilities;
using AppCore.DependencyInjection.Microsoft.Extensions;
using AppCore.Diagnostics;
using AppCore.Hosting;
using AppCore.Hosting.Microsoft.Extensions;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.Hosting
{
    /// <summary>
    /// Provides extension methods for the <see cref="IHostBuilder"/>.
    /// </summary>
    public static class AppCoreHostBuilderExtensions
    {
        private static void InitFacilityActivator(HostBuilderContext context)
        {
            Type key = typeof(IActivator);
            if (!context.Properties.TryGetValue(key, out object _))
            {
                IContainer container = new StartupContainer(context.HostingEnvironment, context.Configuration);
                var activator = container.Resolve<IActivator>();
                context.Properties.Add(key, activator);
                Facility.Activator = activator;
            }
        }

        /// <summary>
        /// Configures the host to register AppCore components with the DI container. This can be called multiple times.
        /// </summary>
        /// <param name="builder">The <see cref="IHostBuilder"/>.</param>
        /// <param name="configureAction">The action which is invoked to configure the <see cref="IComponentRegistry"/>.</param>
        /// <returns>The same instance of the <see cref="IHostBuilder"/>.</returns>
        public static IHostBuilder ConfigureAppCore(
            this IHostBuilder builder,
            Action<HostBuilderContext, IComponentRegistry> configureAction = null)
        {
            Ensure.Arg.NotNull(builder, nameof(builder));

            return builder.ConfigureServices(
                (context, services) =>
                {
                    InitFacilityActivator(context);

                    var registry = new MicrosoftComponentRegistry(services);
                    configureAction?.Invoke(context, registry);

                    registry.AddLogging(l => l.UseMicrosoftExtensions());
                    registry.AddHosting(h => h.UseMicrosoftExtensions());
                });
        }

        /// <summary>
        /// Configures the host to register AppCore components with the DI container. This can be called multiple times.
        /// </summary>
        /// <param name="builder">The <see cref="IHostBuilder"/>.</param>
        /// <param name="configureAction">The action which is invoked to configure the <see cref="IComponentRegistry"/>.</param>
        /// <returns>The same instance of the <see cref="IHostBuilder"/>.</returns>
        public static IHostBuilder ConfigureAppCore(
            this IHostBuilder builder,
            Action<IComponentRegistry> configureAction)
        {
            return builder.ConfigureAppCore((_, services) => configureAction?.Invoke(services));
        }
    }
}
