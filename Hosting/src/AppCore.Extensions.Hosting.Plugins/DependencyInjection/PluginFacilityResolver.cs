// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System.Collections.Generic;
using AppCore.Diagnostics;
using AppCore.Extensions.Hosting.Plugins;

// ReSharper disable once CheckNamespace
namespace AppCore.Extensions.DependencyInjection.Facilities;

/// <summary>
/// Builds an <see cref="IEnumerable{T}"/> of <see cref="Facility"/> by scanning plugin assemblies.
/// </summary>
public class PluginFacilityResolver : IFacilityResolver
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
    IEnumerable<Facility> IFacilityResolver.Resolve()
    {
        foreach (IPluginService<Facility> facility in _pluginManager.GetServices<Facility>())
        {
            yield return facility.Instance;
        }
    }
}