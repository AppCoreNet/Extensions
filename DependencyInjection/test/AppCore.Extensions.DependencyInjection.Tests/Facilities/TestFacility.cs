using Microsoft.Extensions.DependencyInjection;

namespace AppCore.Extensions.DependencyInjection.Facilities;

public class TestFacility : IFacility
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddTransient<FacilityTestService>();
    }
}