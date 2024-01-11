// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using AppCoreNet.Extensions.DependencyInjection.Facilities;
using Microsoft.Extensions.DependencyInjection;

namespace AppCore.Extensions.Hosting.Plugins.TestPlugin2;

public class TestFacility : IFacility
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddTransient<TestFacilityService>();
    }
}