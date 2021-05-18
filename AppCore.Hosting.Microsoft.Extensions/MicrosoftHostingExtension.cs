// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using AppCore.DependencyInjection;
using AppCore.DependencyInjection.Facilities;
using AppCore.Hosting.Microsoft.Extensions;
using Microsoft.Extensions.Hosting;

// ReSharper disable once CheckNamespace
namespace AppCore.Hosting
{
    /// <summary>
    /// Implements Microsoft.Extensions.Hosting facility extension.
    /// </summary>
    public class MicrosoftHostingExtension : FacilityExtension
    {
        /// <inheritdoc/>
        protected override void Build(IComponentRegistry registry)
        {
            base.Build(registry);

            registry.TryAdd(ComponentRegistration.Transient<IApplicationLifetime, MicrosoftHostingApplicationLifetime>());
            registry.TryAdd(ComponentRegistration.Transient<IHostedService, MicrosoftHostingAdapter>());
        }
    }
}