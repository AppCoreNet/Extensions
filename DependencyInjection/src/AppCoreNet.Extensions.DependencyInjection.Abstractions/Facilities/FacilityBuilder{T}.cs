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
/// <typeparam name="T">The type of the facility.</typeparam>
public sealed class FacilityBuilder<T>
    where T : IFacility
{
    /// <summary>
    /// Gets the <see cref="IServiceCollection"/>.
    /// </summary>
    public IServiceCollection Services { get; }

    /// <summary>
    /// Gets the <see cref="IActivator"/>.
    /// </summary>
    public IActivator Activator { get; }

    internal FacilityBuilder(IServiceCollection services, IActivator activator)
    {
        Services = services;
        Activator = activator;
    }

    /// <summary>
    /// Adds an extension to the facility.
    /// </summary>
    /// <remarks>
    /// The type <paramref name="extensionType"/> must implement <see cref="IFacilityExtension{T}"/> with the
    /// type of the facility.
    /// </remarks>
    /// <param name="extensionType">The type of the extension.</param>
    /// <returns>The <see cref="FacilityBuilder{T}"/> to allow chaining.</returns>
    /// <exception cref="ArgumentNullException">Argument <paramref name="extensionType"/> is null.</exception>
    /// <exception cref="ArgumentException">Argument <paramref name="extensionType"/> does not implement <see cref="IFacilityExtension{T}"/> with the type of the facility.</exception>
    public FacilityBuilder<T> AddExtension(
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type extensionType)
    {
        Ensure.Arg.OfType<IFacilityExtension<T>>(extensionType);

        var extension = (IFacilityExtension<T>)Activator.CreateInstance(extensionType)!;
        extension.ConfigureServices(Services);
        return this;
    }

    /// <summary>
    /// Adds an extension to the facility.
    /// </summary>
    /// <typeparam name="TExtension">The type of the extension.</typeparam>
    /// <returns>The <see cref="FacilityBuilder{T}"/> to allow chaining.</returns>
    public FacilityBuilder<T> AddExtension<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TExtension>()
        where TExtension : class, IFacilityExtension<T>
    {
        IFacilityExtension<T> extension = Activator.CreateInstance<TExtension>()!;
        extension.ConfigureServices(Services);
        return this;
    }

    /// <summary>
    /// Adds an extension to the facility.
    /// </summary>
    /// <param name="extension">The <see cref="IFacilityExtension{T}"/>.</param>
    /// <returns>The <see cref="FacilityBuilder{T}"/> to allow chaining.</returns>
    /// <exception cref="ArgumentNullException">Argument <paramref name="extension"/> is null.</exception>
    public FacilityBuilder<T> AddExtension(IFacilityExtension<T> extension)
    {
        Ensure.Arg.NotNull(extension);
        extension.ConfigureServices(Services);
        return this;
    }

    /// <summary>
    /// Adds facility extensions using a <see cref="IFacilityExtensionReflectionBuilder"/> to the
    /// <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="configure">The delegate used to configure the facility extension resolvers.</param>
    /// <returns>The <see cref="FacilityBuilder{T}"/>.</returns>
    /// <exception cref="ArgumentNullException">Argument <paramref name="configure"/> is null.</exception>
    public FacilityBuilder<T> AddExtensionsFrom(Action<IFacilityExtensionReflectionBuilder> configure)
    {
        Ensure.Arg.NotNull(configure);

        var reflectionBuilder = new FacilityExtensionReflectionBuilder(Activator);
        configure(reflectionBuilder);

        foreach (IFacilityExtension extension in reflectionBuilder.Resolve(typeof(T)))
        {
            extension.ConfigureServices(Services);
        }

        return this;
    }
}