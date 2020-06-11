// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using AppCore.DependencyInjection;
using AppCore.DependencyInjection.Facilities;
using Microsoft.Extensions.Hosting;

namespace AppCore.Hosting.Microsoft.Extensions
{
    /// <summary>
    /// Implements Microsoft.Extensions.Hosting facility extension.
    /// </summary>
    public class MicrosoftHostingExtension : FacilityExtension<IHostingFacility>
    {
        /// <inheritdoc />
        protected override void RegisterComponents(IComponentRegistry registry, IHostingFacility facility)
        {
            registry.Register<IApplicationLifetime>()
                    .Add<MicrosoftHostingApplicationLifetime>()
                    .IfNoneRegistered()
                    .PerDependency();

            registry.Register<IHostedService>()
                    .Add<MicrosoftHostingAdapter>()
                    .IfNotRegistered()
                    .PerDependency();
        }
    }
}