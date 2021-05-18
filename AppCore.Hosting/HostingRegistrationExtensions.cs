// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using AppCore.DependencyInjection.Facilities;

// ReSharper disable once CheckNamespace
namespace AppCore.DependencyInjection
{
    /// <summary>
    /// Provides extension methods to register the <see cref="HostingFacility"/>.
    /// </summary>
    public static class HostingRegistrationExtensions
    {
        /// <summary>
        /// Adds the <see cref="HostingFacility"/> to the <see cref="IComponentRegistry"/>.
        /// </summary>
        /// <param name="registry">The <see cref="IComponentRegistry"/>.</param>
        /// <param name="configure">The configuration delegate.</param>
        /// <returns>The <see cref="IComponentRegistry"/>.</returns>
        public static IComponentRegistry AddHosting(this IComponentRegistry registry, Action<HostingFacility> configure = null)
        {
            return registry.AddFacility(configure);
        }
    }
}