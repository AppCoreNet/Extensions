﻿using AppCore.Extensions.DependencyInjection.Facilities;
using Microsoft.Extensions.DependencyInjection;

namespace AppCore.Extensions.DependencyInjection;

public class Facility1Extension1 : IFacilityExtension<Facility1>
{
    public void ConfigureServices(IServiceCollection services)
    {
    }
}