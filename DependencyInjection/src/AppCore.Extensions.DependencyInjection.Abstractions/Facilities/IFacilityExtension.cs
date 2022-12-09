// Licensed under the MIT License.
// Copyright (c) 2018-2022 the AppCore .NET project.

using Microsoft.Extensions.DependencyInjection;

namespace AppCore.Extensions.DependencyInjection.Facilities;

/// <summary>
/// Represents an extension for a facility.
/// </summary>
public interface IFacilityExtension<out T>
    where T : IFacility
{
    /// <summary>
    /// Must be implemented to register services with the <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    void ConfigureServices(IServiceCollection services);
}