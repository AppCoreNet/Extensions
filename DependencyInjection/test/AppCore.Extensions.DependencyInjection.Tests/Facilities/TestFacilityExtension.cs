using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AppCore.Extensions.DependencyInjection.Facilities;

public class TestFacilityExtension : IFacilityExtension<TestFacility>
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.TryAddTransient<FacilityExtensionTestService>();
    }
}