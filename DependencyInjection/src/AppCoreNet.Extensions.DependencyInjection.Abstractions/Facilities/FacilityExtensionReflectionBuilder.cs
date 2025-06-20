// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AppCoreNet.Diagnostics;
using AppCoreNet.Extensions.DependencyInjection.Activator;

namespace AppCoreNet.Extensions.DependencyInjection.Facilities;

internal sealed class FacilityExtensionReflectionBuilder : IFacilityExtensionReflectionBuilder
{
    private readonly IActivator _activator;
    private readonly List<IFacilityExtensionResolver> _resolvers = new();

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

    public IFacilityExtensionReflectionBuilder AddResolver<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(Action<T>? configure = null)
        where T : IFacilityExtensionResolver
    {
        var resolver = _activator.CreateInstance<T>()!;
        configure?.Invoke(resolver);
        return AddResolver(resolver);
    }

    public IReadOnlyCollection<IFacilityExtension> Resolve(Type facilityType)
    {
        return _resolvers.SelectMany(s => s.Resolve(facilityType))
                         .ToList()
                         .AsReadOnly();
    }
}