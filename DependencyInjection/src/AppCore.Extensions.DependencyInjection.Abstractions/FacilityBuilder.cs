// Licensed under the MIT License.
// Copyright (c) 2018-2022 the AppCore .NET project.

using System;
using AppCore.Diagnostics;
using AppCore.Extensions.DependencyInjection.Activator;
using AppCore.Extensions.DependencyInjection.Facilities;
using Microsoft.Extensions.DependencyInjection;

namespace AppCore.Extensions.DependencyInjection;

/// <summary>
/// Provides a builder for facilities.
/// </summary>
public abstract class FacilityBuilder
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
    /// Must be implemented to add an extension to the facility.
    /// </summary>
    /// <param name="extensionType">The type of the extension.</param>
    protected abstract void AddExtensionCore(Type extensionType);

    /// <summary>
    /// Adds an extension to the facility.
    /// </summary>
    /// <remarks>
    /// The type <paramref name="extensionType"/> must implement <see cref="IFacilityExtension{T}"/> with the
    /// type of the facility.
    /// </remarks>
    /// <param name="extensionType">The type of the extension.</param>
    /// <returns>The <see cref="FacilityBuilder"/> to allow chaining.</returns>
    public FacilityBuilder AddExtension(Type extensionType)
    {
        AddExtensionCore(extensionType);
        return this;
    }
}

/// <summary>
/// Provides a builder for facilities.
/// </summary>
public sealed class FacilityBuilder<T> : FacilityBuilder
    where T : IFacility
{
    internal FacilityBuilder(IServiceCollection services, IActivator activator)
        : base(services, activator, typeof(T))
    {
    }

    /// <inheritdoc />
    protected override void AddExtensionCore(Type extensionType)
    {
        Ensure.Arg.NotNull(extensionType);
        Ensure.Arg.OfType(extensionType, typeof(IFacilityExtension<T>));

        var extension = (IFacilityExtension<T>)Activator.CreateInstance(extensionType);
        extension.ConfigureServices(Services);
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
    public new FacilityBuilder<T> AddExtension(Type extensionType)
    {
        AddExtensionCore(extensionType);
        return this;
    }

    /// <summary>
    /// Adds an extension to the facility.
    /// </summary>
    /// <typeparam name="TExtension">The type of the extension.</typeparam>
    /// <returns>The <see cref="FacilityBuilder{T}"/> to allow chaining.</returns>
    public FacilityBuilder<T> AddExtension<TExtension>()
        where TExtension : IFacilityExtension<T>
    {
        return AddExtension(typeof(TExtension));
    }
}
