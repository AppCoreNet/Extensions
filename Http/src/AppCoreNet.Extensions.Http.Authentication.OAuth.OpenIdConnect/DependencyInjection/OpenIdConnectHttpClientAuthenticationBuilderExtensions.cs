﻿// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using AppCoreNet.Diagnostics;
using AppCoreNet.Extensions.Http.Authentication.OAuth;
using AppCoreNet.Extensions.Http.Authentication.OAuth.OpenIdConnect;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

// ReSharper disable once CheckNamespace
namespace AppCoreNet.Extensions.DependencyInjection;

/// <summary>
/// Provides extension methods to register OAuth authentication.
/// </summary>
public static class OpenIdConnectHttpClientAuthenticationBuilderExtensions
{
    /// <summary>
    /// Adds OAuth client credentials authentication scheme by inferring the configuration from a OpenID connect
    /// authentication scheme.
    /// </summary>
    /// <param name="builder">The <see cref="IOAuthClientFromAuthenticationSchemeBuilder"/>.</param>
    /// <param name="configure">The delegate to configure the <see cref="OpenIdConnectClientOptions"/>.</param>
    public static void OpenIdConnect(
        this IOAuthClientFromAuthenticationSchemeBuilder builder,
        Action<OpenIdConnectClientOptions>? configure = null)
    {
        Ensure.Arg.NotNull(builder);

        IServiceCollection services = builder.Services;

        services.AddHttpClientAuthentication()
                .AddScheme<
                    OpenIdConnectClientOptions,
                    OAuthParameters,
                    OAuthClientHandler>(builder.Scheme, configure);

        services.TryAddEnumerable(
            new[]
            {
                ServiceDescriptor
                    .Transient<IOAuthOptionsResolver,
                        OpenIdConnectClientOptionsResolver>(),
            });
    }

    /// <summary>
    /// Adds OAuth user authentication by using authentication tokens from a ASP.NET Core
    /// OpenID connect authentication scheme.
    /// </summary>
    /// <param name="builder">The <see cref="IHttpClientAuthenticationBuilder"/>.</param>
    /// <param name="scheme">The name of the client authentication scheme.</param>
    /// <param name="configure">The delegate to configure the <see cref="OpenIdConnectUserOptions"/>.</param>
    /// <returns>The <see cref="IHttpClientAuthenticationBuilder"/> to allow chaining the calls.</returns>
    public static IHttpClientAuthenticationBuilder AddOpenIdConnect(
        this IHttpClientAuthenticationBuilder builder,
        string scheme,
        Action<OpenIdConnectUserOptions>? configure = null)
    {
        Ensure.Arg.NotNull(builder);

        IServiceCollection services = builder.Services;

        services.AddHttpContextAccessor();

        services.AddHttpClientAuthentication()
                .AddScheme<
                    OpenIdConnectUserOptions,
                    OpenIdConnectUserParameters,
                    OpenIdConnectUserHandler>(scheme, configure);

        services.TryAddEnumerable(
            new[]
            {
                ServiceDescriptor.Transient<IOAuthOptionsResolver, OpenIdConnectUserOptionsResolver>(),
            });

        services.TryAdd(new[]
        {
            ServiceDescriptor.Scoped<OpenIdConnectUserTokenService, OpenIdConnectUserTokenService>(),
            ServiceDescriptor.Scoped<OpenIdConnectUserTokenStore, OpenIdConnectUserTokenStore>(),
        });

        return builder;
    }

    /// <summary>
    /// Adds OAuth user authentication by using authentication tokens from a ASP.NET Core
    /// OpenID connect authentication scheme.
    /// </summary>
    /// <param name="builder">The <see cref="IHttpClientAuthenticationBuilder"/>.</param>
    /// <param name="configure">The delegate to configure the <see cref="OpenIdConnectUserOptions"/>.</param>
    /// <returns>The <see cref="IHttpClientAuthenticationBuilder"/> to allow chaining the calls.</returns>
    public static IHttpClientAuthenticationBuilder AddOpenIdConnect(
        this IHttpClientAuthenticationBuilder builder,
        Action<OpenIdConnectUserOptions>? configure = null)
    {
        return AddOpenIdConnect(builder, OpenIdConnectUserDefaults.AuthenticationScheme, configure);
    }
}
