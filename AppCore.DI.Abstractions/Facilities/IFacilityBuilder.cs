// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System.ComponentModel;

namespace AppCore.DependencyInjection.Facilities
{
    /// <summary>
    /// Represents a type to register facilities.
    /// </summary>
    /// <seealso cref="IFacility"/>
    public interface IFacilityBuilder
    {
        /// <summary>
        /// The <see cref="IFacility"/> which is being registered.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        IFacility Facility { get; }
    }

    /// <summary>
    /// Represents a type to register facilities.
    /// </summary>
    /// <typeparam name="TFacility">The type of the facility.</typeparam>
    /// <seealso cref="IFacility"/>
    /// <seealso cref="IFacilityExtension{TFacility}"/>
    public interface IFacilityBuilder<out TFacility> : IFacilityBuilder
        where TFacility : IFacility
    {
        /// <summary>
        /// The <see cref="IFacility"/> which is being registered.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        new TFacility Facility { get; }

        /// <summary>
        /// Add registration of a facility extension.
        /// </summary>
        /// <param name="extension">The <see cref="IFacilityExtension{TFacility}"/> to add.</param>
        /// <returns>The <see cref="IFacilityBuilder{TFacility}"/> to enable method chaining.</returns>
        IFacilityBuilder<TFacility> AddExtension(IFacilityExtension<TFacility> extension);

        /// <summary>
        /// Add registration of a facility extension.
        /// </summary>
        /// <typeparam name="TExtension">The type of <see cref="IFacilityExtension{TFacility}"/> to add.</typeparam>
        /// <returns>The <see cref="IFacilityBuilder{TFacility}"/> to enable method chaining.</returns>
        IFacilityBuilder<TFacility> AddExtension<TExtension>()
            where TExtension : IFacilityExtension<TFacility>, new ();
    }
}