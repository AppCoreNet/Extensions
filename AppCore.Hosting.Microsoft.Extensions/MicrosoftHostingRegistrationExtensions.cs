using AppCore.DependencyInjection.Facilities;
using AppCore.Hosting;
using AppCore.Hosting.Microsoft.Extensions;

// ReSharper disable once CheckNamespace
namespace AppCore.DependencyInjection
{
    public static class MicrosoftHostingRegistrationExtensions
    {
        public static IFacilityBuilder<IBackgroundServiceFacility> AddMicrosoftHosting(
            this IFacilityBuilder<IBackgroundServiceFacility> builder)
        {
            return builder.Add<MicrosoftHostingExtension>();
        }
    }
}
