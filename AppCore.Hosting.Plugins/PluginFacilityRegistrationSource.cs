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
    public class PluginFacilityRegistrationSource : IFacilityRegistrationSource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PluginFacilityRegistrationSource"/> class.
        /// </summary>
        public PluginFacilityRegistrationSource()
        {
        }

        /// <inheritdoc />
        IEnumerable<Facility> IFacilityRegistrationSource.GetFacilities()
        {
            var pluginManager = PluginManager.Instance;
            if (pluginManager == null)
                throw new InvalidOperationException("Please add the 'PluginHostingFacility' to the DI container before registering components.");

            foreach (IPluginService<Facility> facility in pluginManager.ResolveAll<Facility>())
            {
                yield return facility.Instance;
            }
        }
    }
}