// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using AppCoreNet.Diagnostics;
using AppCoreNet.Extensions.Hosting.Plugins;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace AppCoreNet.Extensions.DependencyInjection;

/// <summary>
/// Provides extension methods to register plugins with MVC.
/// </summary>
[RequiresUnreferencedCode("Uses reflection to discover services.")]
[RequiresDynamicCode("Creates the generic plugin service collection.")]
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
    /// <returns>The <see cref="IMvcBuilder"/> to allow chaining.</returns>
    public static IMvcBuilder AddPluginApplicationParts(this IMvcBuilder builder)
    {
        Ensure.Arg.NotNull(builder);

        IServiceCollection services = builder.Services;

        services.AddPlugins();

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