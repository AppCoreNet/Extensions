// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using Microsoft.Extensions.DependencyInjection;

namespace AppCore.Extensions.Hosting.Plugins.TestPlugin;

public class TestFacility : ITestFacility
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddTransient<TestFacilityService>();
    }
}