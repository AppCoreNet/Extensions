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
        public object CreateInstance(Type facilityType)
        {
            Ensure.Arg.NotNull(facilityType, nameof(facilityType));

            if (!typeof(Facility).IsAssignableFrom(facilityType)
                && !typeof(FacilityExtension).IsAssignableFrom(facilityType))
            {
                throw new ArgumentException(
                    $"The type must either derive from '{typeof(Facility)}' or '{typeof(FacilityExtension)}'.");
            }

            return System.Activator.CreateInstance(facilityType);
        }
    }
}