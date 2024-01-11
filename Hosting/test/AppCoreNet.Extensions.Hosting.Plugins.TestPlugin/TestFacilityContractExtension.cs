// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using AppCoreNet.Extensions.DependencyInjection.Facilities;
using Microsoft.Extensions.DependencyInjection;

namespace AppCoreNet.Extensions.Hosting.Plugins.TestPlugin;

public class TestFacilityContractExtension : IFacilityExtension<ITestFacility>
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddTransient<TestFacilityContractExtensionService>();
    }
}