// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

namespace AppCore.DependencyInjection.Facilities
{
    internal class FacilityExtensionBuilder<TFacility, TExtension> : IFacilityExtensionBuilder<TFacility, TExtension>
        where TFacility : IFacility
        where TExtension : IFacilityExtension<TFacility>
    {
        public TExtension FacilityExtension { get; }

        public FacilityExtensionBuilder(TExtension facilityExtension)
        {
            FacilityExtension = facilityExtension;
        }
    }
}