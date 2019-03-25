// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System;
using System.ComponentModel;

namespace AppCore.DependencyInjection.Facilities
{
    /// <summary>
    /// Represents a type to configure facilities.
    /// </summary>
    /// <typeparam name="TFacility">The type of the facility.</typeparam>
    /// <seealso cref="IFacility"/>
    /// <seealso cref="IFacilityExtension{TFacility}"/>
    public interface IFacilityBuilder<out TFacility>
        where TFacility : IFacility
    {
        /// <summary>
        /// The <see cref="IFacility"/> which is being registered.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        TFacility Facility { get; }

        /// <summary>
        /// Add registration of a facility extension.
        /// </summary>
        /// <param name="extension">The <see cref="IFacilityExtension{TFacility}"/> to add.</param>
        /// <param name="configure">The delegate used to configure the extension.</param>
        /// <typeparam name="TExtension">The type of <see cref="IFacilityExtension{TFacility}"/> to add.</typeparam>
        /// <returns>The <see cref="IFacilityBuilder{TFacility}"/> to enable method chaining.</returns>
        IFacilityBuilder<TFacility> AddExtension<TExtension>(
            TExtension extension,
            Action<IFacilityExtensionBuilder<TFacility, TExtension>> configure = null)
            where TExtension : IFacilityExtension<TFacility>;

        /// <summary>
        /// Add registration of a facility extension.
        /// </summary>
        /// <param name="configure">The delegate used to configure the extension.</param>
        /// <typeparam name="TExtension">The type of <see cref="IFacilityExtension{TFacility}"/> to add.</typeparam>
        /// <returns>The <see cref="IFacilityBuilder{TFacility}"/> to enable method chaining.</returns>
        IFacilityBuilder<TFacility> AddExtension<TExtension>(
            Action<IFacilityExtensionBuilder<TFacility, TExtension>> configure = null)
            where TExtension : IFacilityExtension<TFacility>, new();
    }
}