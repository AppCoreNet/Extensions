// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.ComponentModel;
using AppCoreNet.Diagnostics;
using AppCore.Extensions.Http.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

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
    /// <typeparam name="THandler">The type of the <see cref="IAuthenticationSchemeHandler{TParameters}"/>.</typeparam>
    /// <returns>The <see cref="IHttpClientAuthenticationBuilder"/> to allow chaining the calls.</returns>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static IHttpClientAuthenticationBuilder AddScheme<TOptions, TParameters, THandler>(
        this IHttpClientAuthenticationBuilder builder,
        string name,
        Action<TOptions>? configure = null)
        where TOptions : AuthenticationSchemeOptions
        where TParameters : AuthenticationParameters
        where THandler : class, IAuthenticationSchemeHandler<TParameters>
    {
        Ensure.Arg.NotNull(builder);
        Ensure.Arg.NotNull(name);

        IServiceCollection services = builder.Services;

        services.TryAddTransient<THandler>();
        services.TryAddTransient<IAuthenticationSchemeHandler<TParameters>>(
            sp => sp.GetRequiredService<THandler>());

        services.Configure<HttpClientAuthenticationOptions>(
            o => o
                .AddScheme(name, typeof(THandler), typeof(TOptions)));

        if (configure != null)
        {
            services.Configure(name, configure);
        }

        return builder;
    }
}