﻿// Licensed under the MIT license.
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
public class FacilityBuilder
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

    private IFacilityExtension<IFacility> CreateExtension(Type extensionType)
    {
        Type contractType = extensionType.GetInterfaces()
                                         .First(i => i.GetGenericTypeDefinition() == typeof(IFacilityExtension<>))
                                         .GenericTypeArguments[0];

        Type extensionWrapperType = typeof(FacilityExtensionWrapper<>).MakeGenericType(contractType);
        object extension = Activator.CreateInstance(extensionType)!;

        return (IFacilityExtension<IFacility>)System.Activator.CreateInstance(extensionWrapperType, extension)!;
    }

    /// <summary>
    /// Adds an extension to the facility.
    /// </summary>
    /// <remarks>
    /// The type <paramref name="extensionType"/> must implement <see cref="IFacilityExtension{T}"/> with the
    /// type of the facility.
    /// </remarks>
    /// <param name="extensionType">The type of the extension.</param>
    /// <returns>The <see cref="FacilityBuilder"/> to allow chaining.</returns>
    /// <exception cref="ArgumentNullException">Argument <paramref name="extensionType"/> is null.</exception>
    /// <exception cref="ArgumentException">Argument <paramref name="extensionType"/> does not implement <see cref="IFacilityExtension{T}"/> with the type of the facility.</exception>
    public FacilityBuilder AddExtension(Type extensionType)
    {
        Ensure.Arg.NotNull(extensionType);
        Ensure.Arg.OfType(extensionType, typeof(IFacilityExtension<>).MakeGenericType(FacilityType));

        IFacilityExtension<IFacility> extension = CreateExtension(extensionType);
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

        foreach (IFacilityExtension<IFacility> extension in reflectionBuilder.Resolve(FacilityType))
        {
            extension.ConfigureServices(Services);
        }

        return this;
    }
}