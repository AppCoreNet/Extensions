// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using AppCoreNet.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace AppCoreNet.Extensions.DependencyInjection.Activator;

/// <summary>
/// Provides an implementation of <see cref="IActivator"/> which uses <see cref="IServiceProvider"/> to
/// resolve services.
/// </summary>
public class ServiceProviderActivator : IActivator
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceProviderActivator"/> class.
    /// </summary>
    /// <param name="serviceProvider">The <see cref="IServiceProvider"/> used to resolve services.</param>
    public ServiceProviderActivator(IServiceProvider serviceProvider)
    {
        Ensure.Arg.NotNull(serviceProvider);
        _serviceProvider = serviceProvider;
    }

    /// <inheritdoc />
    public object CreateInstance(Type instanceType, params object[] parameters)
    {
        return ActivatorUtilities.CreateInstance(_serviceProvider, instanceType, parameters);
    }
}