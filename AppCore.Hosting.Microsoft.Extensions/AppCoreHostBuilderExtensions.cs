// Licensed under the MIT License.
// Copyright (c) 2020 the AppCore .NET project.

using System;
using AppCore.DependencyInjection;
using AppCore.DependencyInjection.Microsoft.Extensions;
using AppCore.Diagnostics;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.Hosting
{
    /// <summary>
    /// Provides extension methods for the <see cref="IHostBuilder"/>.
    /// </summary>
    public static class AppCoreHostBuilderExtensions
    {
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
                    var registry = new MicrosoftComponentRegistry();
                    configureAction?.Invoke(context, registry);

                    registry.RegisterFacility<HostingFacility>()
                            .UseMicrosoftHosting();

                    registry.RegisterFacility<LoggingFacility>()
                            .UseMicrosoftExtensions();

                    registry.RegisterComponents(services);
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
            Action<IComponentRegistry> configureAction = null)
        {
            return builder.ConfigureAppCore((_, services) => configureAction?.Invoke(services));
        }
    }
}
