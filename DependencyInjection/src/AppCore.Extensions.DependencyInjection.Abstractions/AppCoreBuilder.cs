// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using AppCore.Diagnostics;
using AppCore.Extensions.DependencyInjection.Activator;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AppCore.Extensions.DependencyInjection;

internal class AppCoreBuilder : IAppCoreBuilder
{
    public IServiceCollection Services { get; }

    public AppCoreBuilder(IServiceCollection services)
    {
        Ensure.Arg.NotNull(services);
        Services = services;

        services.TryAddTransient<IActivator, ServiceProviderActivator>();
    }
}