// Licensed under the MIT License.
// Copyright (c) 2018,2019 the AppCore .NET project.

namespace AppCore.DependencyInjection.Facilities
{
    internal sealed class FacilityExtensionBuilder<TFacility, TExtension> : IFacilityExtensionBuilder<TFacility, TExtension>
        where TFacility : IFacility
        where TExtension : IFacilityExtension<TFacility>
    {
        public IFacilityBuilder<TFacility> FacilityBuilder { get; }

        public TFacility Facility => FacilityBuilder.Facility;

        public TExtension Extension { get; }

        public FacilityExtensionBuilder(IFacilityBuilder<TFacility> facilityBuilder, TExtension facilityExtension)
        {
            FacilityBuilder = facilityBuilder;
            Extension = facilityExtension;
        }
    }
}