// Licensed under the MIT License.
// Copyright (c) 2018,2019 the AppCore .NET project.

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
        /// Configures the <see cref="IFacility"/> which is being registered.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        void Configure(Action<TFacility> configure);

        /// <summary>
        /// Adds an extension to the facility.
        /// </summary>
        /// <param name="extension">The <see cref="IFacilityExtension{TFacility}"/> to add.</param>
        /// <typeparam name="TExtension">The type of <see cref="IFacilityExtension{TFacility}"/> to add.</typeparam>
        /// <returns>The <see cref="IFacilityBuilder{TFacility}"/> to enable method chaining.</returns>
        IFacilityBuilder<TFacility> Add<TExtension>(TExtension extension)
            where TExtension : IFacilityExtension<TFacility>;

        /// <summary>
        /// Adds an extension to the facility.
        /// </summary>
        /// <param name="configure">The delegate which is invoked to configure the extension.</param>
        /// <typeparam name="TExtension">The type of <see cref="IFacilityExtension{TFacility}"/> to add.</typeparam>
        /// <returns>The <see cref="IFacilityExtensionBuilder{TFacility, TExtension}"/> to enable method chaining.</returns>
        IFacilityBuilder<TFacility> Add<TExtension>(
            Action<IFacilityExtensionBuilder<TFacility, TExtension>> configure = null)
            where TExtension : IFacilityExtension<TFacility>, new();
    }
}