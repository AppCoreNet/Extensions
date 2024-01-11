// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using Microsoft.Extensions.DependencyInjection;

namespace AppCoreNet.Extensions.Hosting.Plugins.TestPlugin;

public class TestFacility : ITestFacility
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddTransient<TestFacilityService>();
    }
}