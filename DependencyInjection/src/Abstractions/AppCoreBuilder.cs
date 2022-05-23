// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using AppCore.DependencyInjection.Activator;
using AppCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AppCore.DependencyInjection
{
    internal class AppCoreBuilder : IAppCoreBuilder
    {
        public IServiceCollection Services { get; }

        public AppCoreBuilder(IServiceCollection services)
        {
            Ensure.Arg.NotNull(services, nameof(services));
            Services = services;

            services.TryAddTransient<IActivator, ServiceProviderActivator>();
        }
    }
}