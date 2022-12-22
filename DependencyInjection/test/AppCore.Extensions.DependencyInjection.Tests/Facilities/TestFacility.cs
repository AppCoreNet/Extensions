// Licensed under the MIT License.
// Copyright (c) 2018-2022 the AppCore .NET project.

using Microsoft.Extensions.DependencyInjection;

namespace AppCore.Extensions.DependencyInjection.Facilities;

public class TestFacility : ITestFacility
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddTransient<FacilityTestService>();
    }
}