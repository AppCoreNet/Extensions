// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System.Collections.Generic;
using System.Linq;
using AppCore.Diagnostics;

namespace AppCore.DependencyInjection.Builder
{
    internal class FacilityBuilder : IFacilityBuilder
    {
        public IComponentRegistry Registry { get; }

        public IFacility Facility { get; }

        public FacilityBuilder(IComponentRegistry registry, IFacility facility)
        {
            Registry = registry;
            Facility = facility;
        }

        public virtual IEnumerable<ComponentRegistration> BuildRegistrations()
        {
            return Facility.GetComponentRegistrations();
        }
    }

    internal class FacilityBuilder<TFacility> : FacilityBuilder, IFacilityBuilder<TFacility>
        where TFacility : IFacility
    {
        private readonly List<IFacilityExtension<TFacility>> _extensions = new List<IFacilityExtension<TFacility>>();

        public new TFacility Facility => (TFacility) ((IFacilityBuilder)this).Facility;

        public FacilityBuilder(IComponentRegistry registry, TFacility facility)
            : base(registry, facility)
        {
        }

        public override IEnumerable<ComponentRegistration> BuildRegistrations()
        {
            return base.BuildRegistrations()
                       .Concat(_extensions.SelectMany(e => e.GetComponentRegistrations(Facility)));
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