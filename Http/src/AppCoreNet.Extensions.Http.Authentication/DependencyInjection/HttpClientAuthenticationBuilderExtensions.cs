﻿// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using AppCoreNet.Diagnostics;
using AppCoreNet.Extensions.Http.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

// ReSharper disable once CheckNamespace
namespace AppCoreNet.Extensions.DependencyInjection;

/// <summary>
/// Provides extension methods for the <see cref="IHttpClientAuthenticationBuilder"/>.
/// </summary>
public static class HttpClientAuthenticationBuilderExtensions
{
    /// <summary>
    /// Adds an authentication scheme.
    /// </summary>
    /// <param name="builder">The <see cref="IHttpClientAuthenticationBuilder"/>.</param>
    /// <param name="name">The name of the authentication scheme.</param>
    /// <param name="configure">The option configuration delegate.</param>
    /// <typeparam name="TOptions">The type of the <see cref="AuthenticationSchemeOptions"/>.</typeparam>
    /// <typeparam name="TParameters">The type of the <see cref="AuthenticationParameters"/>.</typeparam>
    /// <typeparam name="THandler">The type of the <see cref="IAuthenticationSchemeHandler"/>.</typeparam>
    /// <returns>The <see cref="IHttpClientAuthenticationBuilder"/> to allow chaining the calls.</returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static IHttpClientAuthenticationBuilder AddScheme<TOptions, TParameters, [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] THandler>(
        this IHttpClientAuthenticationBuilder builder,
        string name,
        Action<TOptions>? configure = null)
        where TOptions : AuthenticationSchemeOptions
        where TParameters : AuthenticationParameters
        where THandler : class, IAuthenticationSchemeHandler
    {
        Ensure.Arg.NotNull(builder);
        Ensure.Arg.NotNull(name);

        IServiceCollection services = builder.Services;

        services.TryAddTransient<THandler>();
        services.TryAddTransient<IAuthenticationSchemeHandler>(
            sp => sp.GetRequiredService<THandler>());

        services.Configure<HttpClientAuthenticationOptions>(
            o => o
                .AddScheme(name, typeof(THandler), typeof(TOptions), typeof(TParameters)));

        if (configure != null)
        {
            services.Configure(name, configure);
        }

        services.TryAddEnumerable(
            new[]
            {
                ServiceDescriptor
                    .Singleton<IPostConfigureOptions<TOptions>, PostConfigureAuthenticationSchemeOptions<TOptions>>(
                        sp => new PostConfigureAuthenticationSchemeOptions<TOptions>(
                            sp.GetRequiredService<TimeProvider>())),
            });

        return builder;
    }
}