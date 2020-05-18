// Licensed under the MIT License.
// Copyright (c) 2018-2020 the AppCore .NET project.

using AppCore.DependencyInjection.Facilities;
using AppCore.Hosting;
using AppCore.Hosting.Microsoft.Extensions;

// ReSharper disable once CheckNamespace
namespace AppCore.DependencyInjection
{
    public static class MicrosoftHostingRegistrationExtensions
    {
        public static IFacilityBuilder<IHostingFacility> AddMicrosoftHosting(
            this IFacilityBuilder<IHostingFacility> builder)
        {
            return builder.Add<MicrosoftHostingExtension>();
        }
    }
}
