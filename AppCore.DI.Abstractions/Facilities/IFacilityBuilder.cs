// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System.ComponentModel;

namespace AppCore.DependencyInjection.Facilities
{
    /// <summary>
    /// Represents a type to build a facility.
    /// </summary>
    public interface IFacilityBuilder<out T> where T : Facility
    {
        /// <summary>
        /// The <see cref="Facility"/>.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        T Facility { get; }
    }
}