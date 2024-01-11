// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Linq;
using AppCoreNet.Diagnostics;
using AppCoreNet.Extensions.DependencyInjection.Activator;

namespace AppCoreNet.Extensions.DependencyInjection.Facilities;

internal sealed class FacilityExtensionReflectionBuilder : IFacilityExtensionReflectionBuilder
{
    private readonly IActivator _activator;
    private readonly List<IFacilityExtensionResolver> _resolvers = new ();

    public FacilityExtensionReflectionBuilder(IActivator activator)
    {
        _activator = activator;
    }

    public IFacilityExtensionReflectionBuilder AddResolver(IFacilityExtensionResolver resolver)
    {
        Ensure.Arg.NotNull(resolver);
        _resolvers.Add(resolver);
        return this;
    }

    public IFacilityExtensionReflectionBuilder AddResolver<T>(Action<T>? configure = null)
        where T : IFacilityExtensionResolver
    {
        var resolver = _activator.CreateInstance<T>();
        configure?.Invoke(resolver);
        return AddResolver(resolver);
    }

    public IReadOnlyCollection<IFacilityExtension<IFacility>> Resolve(Type facilityType)
    {
        Type[] facilityTypes = facilityType.GetTypesAssignableFrom()
                                           .Where(t => t.GetInterfaces().Contains(typeof(IFacility)))
                                           .ToArray();

        return _resolvers.SelectMany(s => facilityTypes.SelectMany(s.Resolve))
                         .ToList()
                         .AsReadOnly();
    }
}