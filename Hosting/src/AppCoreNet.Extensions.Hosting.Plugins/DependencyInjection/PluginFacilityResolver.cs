// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AppCoreNet.Diagnostics;
using AppCoreNet.Extensions.Hosting.Plugins;

// ReSharper disable once CheckNamespace
namespace AppCoreNet.Extensions.DependencyInjection.Facilities;

/// <summary>
/// Builds an <see cref="IEnumerable{T}"/> of <see cref="IFacility"/> by scanning plugin assemblies.
/// </summary>
[RequiresUnreferencedCode("Uses reflection to discover types.")]
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

    /// <inheritdoc />
    IEnumerable<IFacilityExtension> IFacilityExtensionResolver.Resolve(Type facilityType)
    {
        IPluginServiceCollection<object> services = _pluginManager.GetServices(typeof(IFacilityExtension));

        foreach (IPluginService<object> service in services)
        {
            if (!IsCompatibleExtensionType(service.Instance.GetType(), facilityType))
                continue;

            yield return (IFacilityExtension)service.Instance;
        }

        static bool IsCompatibleExtensionType(Type extensionType, Type facilityType)
        {
            IEnumerable<Type> extensionInterfaces =
                extensionType.GetInterfaces()
                             .Where(t => t.IsGenericType
                                         && t.GetGenericTypeDefinition() == typeof(IFacilityExtension<>));

            IEnumerable<Type> extendedFacilityTypes = extensionInterfaces.Select(t => t.GenericTypeArguments[0]);
            return extendedFacilityTypes.Any(t => t.IsAssignableFrom(facilityType));
        }
    }
}