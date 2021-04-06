// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

namespace AppCore.DependencyInjection.Facilities
{
    internal sealed class FacilityBuilder<T> : IFacilityBuilder<T>
        where T : Facility
    {
        public IComponentRegistry Registry { get; }

        public T Facility { get; }

        public FacilityBuilder(IComponentRegistry registry, T facility)
        {
            Registry = registry;
            Facility = facility;
        }

        public void Build()
        {
            Facility.Build(Registry);
        }
    }
}