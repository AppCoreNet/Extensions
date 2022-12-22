using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AppCore.Extensions.DependencyInjection.Facilities;

public class TestFacilityContractExtension : IFacilityExtension<ITestFacility>
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddTransient<FacilityExtensionTestService>();
    }
}