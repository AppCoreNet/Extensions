// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using Microsoft.Extensions.DependencyInjection;

namespace AppCore.Extensions.DependencyInjection.Facilities;

internal sealed class FacilityExtensionWrapper<TContract> : IFacilityExtension<IFacility>
    where TContract : IFacility
{
    internal IFacilityExtension<TContract> Extension { get; }

    public FacilityExtensionWrapper(IFacilityExtension<TContract> extension)
    {
        Extension = extension;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        Extension.ConfigureServices(services);
    }
}