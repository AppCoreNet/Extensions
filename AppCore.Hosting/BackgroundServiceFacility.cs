using AppCore.DependencyInjection.Facilities;
using AppCore.Hosting;

// ReSharper disable once CheckNamespace
namespace AppCore.DependencyInjection
{
    public class BackgroundServiceFacility : Facility, IBackgroundServiceFacility
    {
        protected override void RegisterComponents(IComponentRegistry registry)
        {
        }
    }
}
