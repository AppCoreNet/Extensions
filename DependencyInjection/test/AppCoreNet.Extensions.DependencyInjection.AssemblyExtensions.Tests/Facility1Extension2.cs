// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using AppCoreNet.Extensions.DependencyInjection.Facilities;
using Microsoft.Extensions.DependencyInjection;

namespace AppCoreNet.Extensions.DependencyInjection;

public class Facility1Extension2 : IFacilityExtension<IFacility1>
{
    public void ConfigureServices(IServiceCollection services)
    {
    }
}