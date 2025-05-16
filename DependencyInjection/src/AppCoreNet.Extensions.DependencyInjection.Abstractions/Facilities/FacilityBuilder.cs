// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Diagnostics.CodeAnalysis;
using AppCoreNet.Diagnostics;
using AppCoreNet.Extensions.DependencyInjection.Activator;
using Microsoft.Extensions.DependencyInjection;

namespace AppCoreNet.Extensions.DependencyInjection.Facilities;

/// <summary>
/// Provides a builder for facilities.
/// </summary>
public sealed class FacilityBuilder
{
    /// <summary>
    /// Gets the <see cref="IServiceCollection"/>.
    /// </summary>
    public IServiceCollection Services { get; }

    /// <summary>
    /// Gets the <see cref="IActivator"/>.
    /// </summary>
    public IActivator Activator { get; }

    /// <summary>
    /// Gets the type of the facility.
    /// </summary>
    public Type FacilityType { get; }

    internal FacilityBuilder(IServiceCollection services, IActivator activator, Type facilityType)
    {
        Services = services;
        Activator = activator;
        FacilityType = facilityType;
    }

    /// <summary>
    /// Adds an extension to the facility.
    /// </summary>
    /// <remarks>
    /// The type <paramref name="extensionType"/> must implement <see cref="IFacilityExtension"/>.
    /// </remarks>
    /// <param name="extensionType">The type of the extension.</param>
    /// <returns>The <see cref="FacilityBuilder"/> to allow chaining.</returns>
    /// <exception cref="ArgumentNullException">Argument <paramref name="extensionType"/> is null.</exception>
    /// <exception cref="ArgumentException">Argument <paramref name="extensionType"/> does not implement <see cref="IFacilityExtension"/> with the type of the facility.</exception>
    public FacilityBuilder AddExtension(
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type extensionType)
    {
        Ensure.Arg.NotNull(extensionType);
        Ensure.Arg.OfType<IFacilityExtension>(extensionType);

        var extension = (IFacilityExtension)Activator.CreateInstance(extensionType)!;
        extension.ConfigureServices(Services);
        return this;
    }

    /// <summary>
    /// Adds facility extensions using a <see cref="IFacilityExtensionReflectionBuilder"/> to the <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="configure">The delegate used to configure the facility extension resolvers.</param>
    /// <returns>The <see cref="FacilityBuilder"/>.</returns>
    /// <exception cref="ArgumentNullException">Argument <paramref name="configure"/> is null.</exception>
    public FacilityBuilder AddExtensionsFrom(Action<IFacilityExtensionReflectionBuilder> configure)
    {
        Ensure.Arg.NotNull(configure);

        var reflectionBuilder = new FacilityExtensionReflectionBuilder(Activator);
        configure(reflectionBuilder);

        foreach (IFacilityExtension extension in reflectionBuilder.Resolve(FacilityType))
        {
            extension.ConfigureServices(Services);
        }

        return this;
    }
}