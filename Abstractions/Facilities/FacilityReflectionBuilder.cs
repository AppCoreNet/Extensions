// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Linq;
using AppCore.DependencyInjection.Activator;

namespace AppCore.DependencyInjection.Facilities
{
    internal class FacilityReflectionBuilder : IFacilityReflectionBuilder
    {
        private readonly List<IFacilityResolver> _resolvers = new();

        public IFacilityReflectionBuilder AddResolver(IFacilityResolver resolver)
        {
            _resolvers.Add(resolver);
            return this;
        }

        public IFacilityReflectionBuilder AddResolver<T>(Action<T> configure = null)
            where T : IFacilityResolver, new()
        {
            var source = new T();
            configure?.Invoke(source);
            return AddResolver(source);
        }

        public IReadOnlyCollection<Facility> Resolve(IActivator activator)
        {
            return _resolvers.SelectMany(s => s.Resolve(activator))
                             .ToList()
                             .AsReadOnly();
        }
    }
}