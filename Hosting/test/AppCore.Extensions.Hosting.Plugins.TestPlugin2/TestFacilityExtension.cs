// Licensed under the MIT License.
// Copyright (c) 2018-2022 the AppCore .NET project.

using AppCore.Extensions.DependencyInjection.Facilities;
using Microsoft.Extensions.DependencyInjection;

namespace AppCore.Extensions.Hosting.Plugins.TestPlugin2;

public class TestFacilityExtension : IFacilityExtension<TestFacility>
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddTransient<TestFacilityExtensionService>();
    }
}