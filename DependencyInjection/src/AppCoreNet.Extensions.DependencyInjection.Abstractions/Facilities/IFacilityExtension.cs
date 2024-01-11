// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using Microsoft.Extensions.DependencyInjection;

namespace AppCoreNet.Extensions.DependencyInjection.Facilities;

/// <summary>
/// Represents an extension for a facility.
/// </summary>
/// <typeparam name="T">The type of the facility.</typeparam>
public interface IFacilityExtension<in T>
    where T : IFacility
{
    /// <summary>
    /// Must be implemented to register services with the <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    void ConfigureServices(IServiceCollection services);
}