// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using AppCore.Extensions.DependencyInjection.Facilities;
using Microsoft.Extensions.DependencyInjection;

namespace AppCore.Extensions.Hosting.Plugins.TestPlugin;

public class TestFacilityContractExtension : IFacilityExtension<ITestFacility>
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddTransient<TestFacilityContractExtensionService>();
    }
}