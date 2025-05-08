// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Linq;
using AppCoreNet.Diagnostics;
using AppCoreNet.Extensions.DependencyInjection.Activator;
using Microsoft.Extensions.DependencyInjection;

namespace AppCoreNet.Extensions.DependencyInjection.Facilities;

/// <summary>
/// Provides a builder for facilities.
/// </summary>
/// <typeparam name="T">The type of the facility.</typeparam>
public sealed class FacilityBuilder<T> : FacilityBuilder
    where T : IFacility
{
    internal FacilityBuilder(IServiceCollection services, IActivator activator)
        : base(services, activator, typeof(T))
    {
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
    public new FacilityBuilder<T> AddExtension(Type extensionType)
    {
        base.AddExtension(extensionType);
        return this;
    }

    /// <summary>
    /// Adds an extension to the facility.
    /// </summary>
    /// <typeparam name="TExtension">The type of the extension.</typeparam>
    /// <returns>The <see cref="FacilityBuilder{T}"/> to allow chaining.</returns>
    public FacilityBuilder<T> AddExtension<TExtension>()
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
    /// Adds facility extensions using a <see cref="IFacilityExtensionReflectionBuilder"/> to the <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="configure">The delegate used to configure the facility extension resolvers.</param>
    /// <returns>The <see cref="FacilityBuilder{T}"/>.</returns>
    /// <exception cref="ArgumentNullException">Argument <paramref name="configure"/> is null.</exception>
    public new FacilityBuilder<T> AddExtensionsFrom(Action<IFacilityExtensionReflectionBuilder> configure)
    {
        base.AddExtensionsFrom(configure);
        return this;
    }
}