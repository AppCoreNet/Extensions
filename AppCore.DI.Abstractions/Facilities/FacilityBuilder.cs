// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System.Collections.Generic;
using AppCore.Diagnostics;

namespace AppCore.DependencyInjection.Facilities
{
    internal class FacilityBuilder : IFacilityBuilder
    {
        public IFacility Facility { get; }

        public FacilityBuilder(IFacility facility)
        {
            Facility = facility;
        }

        public virtual void RegisterComponents(IComponentRegistry registry)
        {
            Facility.RegisterComponents(registry);
        }
    }

    internal class FacilityBuilder<TFacility> : FacilityBuilder, IFacilityBuilder<TFacility>
        where TFacility : IFacility
    {
        private readonly List<IFacilityExtension<TFacility>> _extensions = new List<IFacilityExtension<TFacility>>();

        public new TFacility Facility => (TFacility) ((IFacilityBuilder)this).Facility;

        public FacilityBuilder(TFacility facility)
            : base(facility)
        {
        }

        public override void RegisterComponents(IComponentRegistry registry)
        {
            foreach (IFacilityExtension<TFacility> facilityExtension in _extensions)
            {
                facilityExtension.RegisterComponents(registry, Facility);
            }

            base.RegisterComponents(registry);
        }

        public IFacilityBuilder<TFacility> AddExtension(IFacilityExtension<TFacility> extension)
        {
            Ensure.Arg.NotNull(extension, nameof(extension));
            _extensions.Add(extension);
            return this;
        }

        public IFacilityBuilder<TFacility> AddExtension<TExtension>()
            where TExtension : IFacilityExtension<TFacility>, new()
        {
            return AddExtension(new TExtension());
        }
    }
}