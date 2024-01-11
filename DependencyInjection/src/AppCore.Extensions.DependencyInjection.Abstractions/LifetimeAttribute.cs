// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using Microsoft.Extensions.DependencyInjection;

namespace AppCore.Extensions.DependencyInjection;

/// <summary>
/// Specifies the lifetime of a service when registered via dynamic scanning.
/// </summary>
/// <seealso cref="ServiceLifetime"/>
[AttributeUsage(AttributeTargets.Class)]
public sealed class LifetimeAttribute : Attribute
{
    /// <summary>
    /// Gets the <see cref="ServiceLifetime"/>.
    /// </summary>
    public ServiceLifetime Lifetime { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="LifetimeAttribute"/> class.
    /// </summary>
    /// <param name="lifetime">The lifetime of the service.</param>
    public LifetimeAttribute(ServiceLifetime lifetime)
    {
        Lifetime = lifetime;
    }
}