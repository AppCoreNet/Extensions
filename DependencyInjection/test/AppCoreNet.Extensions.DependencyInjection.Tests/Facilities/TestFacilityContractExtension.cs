// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using Microsoft.Extensions.DependencyInjection;

namespace AppCoreNet.Extensions.DependencyInjection.Facilities;

public class TestFacilityContractExtension : IFacilityExtension<ITestFacility>
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddTransient<FacilityExtensionTestService>();
    }
}