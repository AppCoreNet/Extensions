// Licensed under the MIT License.
// Copyright (c) 2018-2022 the AppCore .NET project.

using System.Collections.Generic;
using System.Reflection;
using AppCore.Diagnostics;
using AppCore.Extensions.DependencyInjection;
using AppCore.Extensions.Hosting.Plugins;
using AppCore.Extensions.Hosting.Plugins.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Provides extension methods to register plugins with MVC.
    /// </summary>
    public static class PluginsMvcBuilderExtensions
    {
        private static void AddApplicationParts(ApplicationPartManager manager, Assembly assembly)
        {
            var partFactory = ApplicationPartFactory.GetApplicationPartFactory(assembly);
            foreach (ApplicationPart part in partFactory.GetApplicationParts(assembly))
            {
                manager.ApplicationParts.Add(part);
            }
        }

        /// <summary>
        /// Adds MVC application parts from loaded plugins.
        /// </summary>
        /// <param name="builder">The <see cref="IMvcBuilder"/>.</param>
        /// <returns>The <see cref="IMvcBuilder"/>.</returns>
        public static IMvcBuilder AddPluginApplicationParts(this IMvcBuilder builder)
        {
            Ensure.Arg.NotNull(builder);

            IServiceCollection services = builder.Services;

            services.AddAppCore()
                    .AddPlugins();

            var serviceProvider = new ServiceCollectionServiceProvider(services);
            var pluginManager = serviceProvider.GetRequiredService<IPluginManager>();

            builder.ConfigureApplicationPartManager(
                manager =>
                {
                    foreach (IPlugin plugin in pluginManager.Plugins)
                    {
                        using (plugin.EnterContextualReflection())
                        {
                            Assembly assembly = plugin.Assembly;
                            AddApplicationParts(manager, assembly);

                            // This piece finds and loads related parts, such as MvcAppPlugin1.Views.dll.
                            IEnumerable<RelatedAssemblyAttribute> relatedAssemblyAttributes =
                                assembly.GetCustomAttributes<RelatedAssemblyAttribute>();

                            foreach (RelatedAssemblyAttribute attr in relatedAssemblyAttributes)
                            {
                                assembly = plugin.LoadAssembly(attr.AssemblyFileName);
                                AddApplicationParts(manager, assembly);
                            }
                        }
                    }
                });

            return builder;
        }
    }
}