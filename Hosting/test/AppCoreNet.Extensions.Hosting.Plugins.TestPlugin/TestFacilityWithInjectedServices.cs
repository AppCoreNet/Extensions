// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using AppCoreNet.Extensions.DependencyInjection.Activator;
using AppCoreNet.Extensions.DependencyInjection.Facilities;
using Microsoft.Extensions.DependencyInjection;

namespace AppCoreNet.Extensions.Hosting.Plugins.TestPlugin;

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