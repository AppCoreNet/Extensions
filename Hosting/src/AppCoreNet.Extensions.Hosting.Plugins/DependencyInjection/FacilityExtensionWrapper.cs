// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using AppCoreNet.Extensions.DependencyInjection.Facilities;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace AppCore.Extensions.DependencyInjection.Facilities;

internal sealed class FacilityExtensionWrapper<TContract> : IFacilityExtension<IFacility>
    where TContract : IFacility
{
    private readonly IFacilityExtension<TContract> _extension;

    public FacilityExtensionWrapper(IFacilityExtension<TContract> extension)
    {
        _extension = extension;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        _extension.ConfigureServices(services);
    }
}