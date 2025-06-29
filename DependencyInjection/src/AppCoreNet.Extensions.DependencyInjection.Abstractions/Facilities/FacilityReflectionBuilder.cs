// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AppCoreNet.Diagnostics;
using AppCoreNet.Extensions.DependencyInjection.Activator;

namespace AppCoreNet.Extensions.DependencyInjection.Facilities;

internal sealed class FacilityReflectionBuilder : IFacilityReflectionBuilder
{
    private readonly IActivator _activator;
    private readonly List<IFacilityResolver> _resolvers = new();
    private Action<IFacilityExtensionReflectionBuilder>? _extensionsConfig;

    public FacilityReflectionBuilder(IActivator activator)
    {
        _activator = activator;
    }

    public IFacilityReflectionBuilder AddResolver(IFacilityResolver resolver)
    {
        Ensure.Arg.NotNull(resolver);
        _resolvers.Add(resolver);
        return this;
    }

    public IFacilityReflectionBuilder AddResolver<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(Action<T>? configure = null)
        where T : IFacilityResolver
    {
        var resolver = _activator.CreateInstance<T>()!;
        configure?.Invoke(resolver);
        return AddResolver(resolver);
    }

    public IFacilityReflectionBuilder AddExtensionsFrom(Action<IFacilityExtensionReflectionBuilder> configure)
    {
        Ensure.Arg.NotNull(configure);
        _extensionsConfig = configure;
        return this;
    }

    public IReadOnlyCollection<(IFacility Facility, IReadOnlyCollection<IFacilityExtension>
        FacilityExtensions)> Resolve()
    {
        List<IFacility> facilities =
            _resolvers.SelectMany(s => s.Resolve())
                      .ToList();

        FacilityExtensionReflectionBuilder? extensionReflectionBuilder = null;

        if (_extensionsConfig != null)
        {
            extensionReflectionBuilder = new FacilityExtensionReflectionBuilder(_activator);
            _extensionsConfig(extensionReflectionBuilder);
        }

        ReadOnlyCollection<IFacilityExtension> emptyFacilityExtensions =
            new List<IFacilityExtension>().AsReadOnly();

        List<(IFacility Facility, IReadOnlyCollection<IFacilityExtension> FacilityExtensions)> result =
            new();

        foreach (IFacility facility in facilities)
        {
            IReadOnlyCollection<IFacilityExtension> facilityExtensions =
                extensionReflectionBuilder?.Resolve(facility.GetType())
                ?? emptyFacilityExtensions;

            result.Add((Facility: facility, FacilityExtensions: facilityExtensions));
        }

        return result;
    }
}