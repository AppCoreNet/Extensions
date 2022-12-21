// Licensed under the MIT License.
// Copyright (c) 2018-2022 the AppCore .NET project.

using Microsoft.Extensions.DependencyInjection;

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