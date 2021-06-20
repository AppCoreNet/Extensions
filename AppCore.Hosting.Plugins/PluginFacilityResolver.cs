// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using System.Collections.Generic;
using AppCore.Hosting.Plugins;

// ReSharper disable once CheckNamespace
namespace AppCore.DependencyInjection.Facilities
{
    /// <summary>
    /// Builds an <see cref="IEnumerable{T}"/> of <see cref="Facility"/> by scanning plugin assemblies. 
    /// </summary>
    public class PluginFacilityResolver : IFacilityResolver
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PluginFacilityResolver"/> class.
        /// </summary>
        public PluginFacilityResolver()
        {
        }

        /// <inheritdoc />
        IEnumerable<Facility> IFacilityResolver.Resolve()
        {
            PluginManager pluginManager = PluginFacility.PluginManager;
            if (pluginManager == null)
                throw new InvalidOperationException("Please add the 'PluginFacility' to the DI container before registering components.");

            foreach (IPluginService<Facility> facility in pluginManager.GetServices<Facility>())
            {
                yield return facility.Instance;
            }
        }
    }
}