// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using AppCore.Diagnostics;

namespace AppCore.DependencyInjection.Facilities
{
    /// <summary>
    /// Represents the default factory for facilities.
    /// </summary>
    public sealed class DefaultFacilityActivator : IFacilityActivator
    {
        /// <summary>
        /// Gets the static instance of the <see cref="DefaultFacilityActivator"/>.
        /// </summary>
        public static DefaultFacilityActivator Instance { get; } = new();

        /// <inheritdoc />
        public Facility CreateInstance(Type facilityType)
        {
            Ensure.Arg.NotNull(facilityType, nameof(facilityType));
            Ensure.Arg.OfType(facilityType, typeof(Facility), nameof(facilityType));

            return (Facility) Activator.CreateInstance(facilityType);
        }
    }
}