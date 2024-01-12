// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

#if NET6_0_OR_GREATER

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace AppCoreNet.Extensions.DependencyInjection;

/// <content>
/// .NET 6.0+ implementation of <see cref="IServiceProvider"/>.
/// </content>
internal sealed partial class ServiceCollectionServiceProvider : IServiceProviderIsService
{
    public bool IsService(Type serviceType)
    {
        if (serviceType == typeof(IServiceProvider) || serviceType == typeof(IServiceProviderIsService))
            return true;

        if (_additionalServices.TryGetValue(serviceType, out object? _))
            return true;

        if (serviceType.IsGenericType && serviceType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            return true;

        return _services.Any(
            sd => sd.ServiceType == serviceType
                  || (serviceType.IsGenericType
                      && sd.ServiceType == serviceType.GetGenericTypeDefinition()));
    }
}

#endif