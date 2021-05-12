// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using AppCore.DependencyInjection;
using AppCore.DependencyInjection.Facilities;
using Microsoft.Extensions.Hosting;

namespace AppCore.Hosting.Microsoft.Extensions
{
    /// <summary>
    /// Implements Microsoft.Extensions.Hosting facility extension.
    /// </summary>
    public class MicrosoftHostingExtension : FacilityExtension
    {
        protected override void Build(IComponentRegistry registry)
        {
            base.Build(registry);

            registry.TryAdd(ComponentRegistration.Transient<IApplicationLifetime, MicrosoftHostingApplicationLifetime>());
            registry.TryAdd(ComponentRegistration.Transient<IHostedService, MicrosoftHostingAdapter>());
        }
    }
}