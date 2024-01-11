// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using AppCore.Extensions.DependencyInjection.Activator;
using AppCore.Extensions.DependencyInjection.Facilities;
using Microsoft.Extensions.DependencyInjection;

namespace AppCore.Extensions.Hosting.Plugins.TestPlugin;

public class TestFacilityWithInjectedServices : IFacility
{
    public TestFacilityWithInjectedServices(IActivator activator)
    {
        if (activator == null)
            throw new ArgumentNullException(nameof(activator));
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddTransient<TestFacilityService>();
    }
}