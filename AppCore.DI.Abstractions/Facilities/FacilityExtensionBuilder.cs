// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

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

        public IFacilityExtensionBuilder<TFacility, TExtension1> AddExtension<TExtension1>(TExtension1 extension)
            where TExtension1 : IFacilityExtension<TFacility>
        {
            return FacilityBuilder.AddExtension(extension);
        }

        public IFacilityExtensionBuilder<TFacility, TExtension1> AddExtension<TExtension1>()
            where TExtension1 : IFacilityExtension<TFacility>, new()
        {
            return FacilityBuilder.AddExtension<TExtension1>();
        }
    }
}