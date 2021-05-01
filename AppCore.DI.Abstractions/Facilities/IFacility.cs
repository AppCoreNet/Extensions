// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;

namespace AppCore.DependencyInjection.Facilities
{
    /// <summary>
    /// Represents a facility.
    /// </summary>
    public interface IFacility
    {
        /// <summary>
        /// Registers a callback which is invoked when the facility is built.
        /// </summary>
        /// <param name="callback">The callback.</param>
        void ConfigureRegistry(Action<IComponentRegistry> callback);

        /// <summary>
        /// Adds an extension to the facility.
        /// </summary>
        /// <param name="extensionType">The type of the facility extension.</param>
        FacilityExtension AddExtension(Type extensionType);

        /// <summary>
        /// Adds an extension to the facility.
        /// </summary>
        /// <typeparam name="T">The type of the facility extension.</typeparam>
        T AddExtension<T>() where T : FacilityExtension;
    }
}