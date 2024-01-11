// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using Microsoft.Extensions.DependencyInjection;

namespace AppCore.Extensions.DependencyInjection.Facilities;

/// <summary>
/// Represents a facility.
/// </summary>
public interface IFacility
{
    /// <summary>
    /// Must be implemented to register services with the <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    void ConfigureServices(IServiceCollection services);
}