// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Linq;
using AppCoreNet.Diagnostics;
using AppCoreNet.Extensions.Hosting.Plugins;

// ReSharper disable once CheckNamespace
namespace AppCoreNet.Extensions.DependencyInjection.Facilities;

/// <summary>
/// Builds an <see cref="IEnumerable{T}"/> of <see cref="IFacility"/> by scanning plugin assemblies.
/// </summary>
public class PluginFacilityResolver : IFacilityResolver, IFacilityExtensionResolver
{
    private readonly IPluginManager _pluginManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="PluginFacilityResolver"/> class.
    /// </summary>
    /// <param name="pluginManager">The <see cref="IPluginManager"/>.</param>
    public PluginFacilityResolver(IPluginManager pluginManager)
    {
        Ensure.Arg.NotNull(pluginManager);
        _pluginManager = pluginManager;
    }

    /// <inheritdoc />
    IEnumerable<IFacility> IFacilityResolver.Resolve()
    {
        foreach (IPluginService<IFacility> facility in _pluginManager.GetServices<IFacility>())
        {
            yield return facility.Instance;
        }
    }

    private IFacilityExtension<IFacility> CreateExtension(IPluginService<object> pluginService)
    {
        Type extensionType = pluginService.GetType()
                                          .GetInterfaces()
                                          .First(i => i.GetGenericTypeDefinition() == typeof(IPluginService<>))
                                          .GenericTypeArguments[0];

        Type contractType = extensionType.GenericTypeArguments[0];
        Type extensionWrapperType = typeof(FacilityExtensionWrapper<>).MakeGenericType(contractType);

        return (IFacilityExtension<IFacility>)System.Activator.CreateInstance(
            extensionWrapperType,
            pluginService.Instance) !;
    }

    /// <inheritdoc />
    IEnumerable<IFacilityExtension<IFacility>> IFacilityExtensionResolver.Resolve(Type facilityType)
    {
        IPluginServiceCollection<object> services = _pluginManager.GetServices(
            typeof(IFacilityExtension<>).MakeGenericType(facilityType));

        foreach (IPluginService<object> service in services)
        {
            yield return CreateExtension(service);
        }
    }
}