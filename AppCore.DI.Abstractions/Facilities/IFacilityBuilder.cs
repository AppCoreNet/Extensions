// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

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
        /// Adds an extension to the facility.
        /// </summary>
        /// <param name="extension">The <see cref="IFacilityExtension{TFacility}"/> to add.</param>
        /// <typeparam name="TExtension">The type of <see cref="IFacilityExtension{TFacility}"/> to add.</typeparam>
        /// <returns>The <see cref="IFacilityExtensionBuilder{TFacility, TExtension}"/> to enable method chaining.</returns>
        IFacilityExtensionBuilder<TFacility, TExtension> AddExtension<TExtension>(TExtension extension)
            where TExtension : IFacilityExtension<TFacility>;

        /// <summary>
        /// Adds an extension to the facility.
        /// </summary>
        /// <typeparam name="TExtension">The type of <see cref="IFacilityExtension{TFacility}"/> to add.</typeparam>
        /// <returns>The <see cref="IFacilityExtensionBuilder{TFacility, TExtension}"/> to enable method chaining.</returns>
        IFacilityExtensionBuilder<TFacility, TExtension> AddExtension<TExtension>()
            where TExtension : IFacilityExtension<TFacility>, new();
    }
}