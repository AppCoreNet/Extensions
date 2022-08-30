// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Linq;
using AppCore.Extensions.DependencyInjection.Activator;

namespace AppCore.Extensions.DependencyInjection.Facilities;

internal class FacilityReflectionBuilder : IFacilityReflectionBuilder
{
    private readonly IActivator _activator;
    private readonly List<IFacilityResolver> _resolvers = new();

    public FacilityReflectionBuilder(IActivator activator)
    {
        _activator = activator;
    }

    public IFacilityReflectionBuilder AddResolver(IFacilityResolver resolver)
    {
        _resolvers.Add(resolver);
        return this;
    }

    public IFacilityReflectionBuilder AddResolver<T>(Action<T>? configure = null)
        where T : IFacilityResolver
    {
        var resolver = _activator.CreateInstance<T>();
        configure?.Invoke(resolver);
        return AddResolver(resolver);
    }

    public IReadOnlyCollection<Facility> Resolve()
    {
        return _resolvers.SelectMany(s => s.Resolve())
                         .ToList()
                         .AsReadOnly();
    }
}