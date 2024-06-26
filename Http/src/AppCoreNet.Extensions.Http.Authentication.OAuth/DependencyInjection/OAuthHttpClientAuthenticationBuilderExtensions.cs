﻿// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Linq;
using AppCoreNet.Diagnostics;
using AppCoreNet.Extensions.Http.Authentication.OAuth;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

// ReSharper disable once CheckNamespace
namespace AppCoreNet.Extensions.DependencyInjection;

/// <summary>
/// Provides extension methods to register OAuth authentication.
/// </summary>
public static class OAuthHttpClientAuthenticationBuilderExtensions
{
    internal sealed class OAuthCoreServiceGuard
    {
    }

    /// <summary>
    /// Adds OAuth client authentication services.
    /// </summary>
    /// <param name="builder">The <see cref="IHttpClientAuthenticationBuilder"/>.</param>
    /// <returns>The <see cref="IHttpClientAuthenticationBuilder"/> to allow chaining the calls.</returns>
    public static IHttpClientAuthenticationBuilder AddOAuthCore(this IHttpClientAuthenticationBuilder builder)
    {
        Ensure.Arg.NotNull(builder);

        IServiceCollection services = builder.Services;

        if (builder.Services.Any(sd => sd.ServiceType == typeof(OAuthCoreServiceGuard)))
            return builder;

        services.AddTransient<OAuthCoreServiceGuard>();

        services.AddDistributedMemoryCache();

        services.AddOptions<OAuthTokenCacheOptions>();
        services.AddSingleton<IOAuthTokenCache, OAuthTokenCache>();
        services.AddSingleton<IOAuthTokenService, OAuthTokenService>();

        services.AddTransient<IOAuthOptionsProvider, OAuthOptionsProvider>();
        services.AddHttpClient<IOAuthTokenClient, OAuthTokenClient>();

        return builder;
    }

    /// <summary>
    /// Adds OAuth client credentials authentication scheme.
    /// </summary>
    /// <param name="builder">The <see cref="IHttpClientAuthenticationBuilder"/>.</param>
    /// <param name="scheme">The name of the client authentication scheme.</param>
    /// <param name="configure">A delegate that configures the <see cref="OAuthClientOptions"/>.</param>
    /// <returns>The <see cref="IHttpClientAuthenticationBuilder"/> to allow chaining the calls.</returns>
    public static IHttpClientAuthenticationBuilder AddOAuthClient(
        this IHttpClientAuthenticationBuilder builder,
        string scheme,
        Action<OAuthClientOptions> configure)
    {
        Ensure.Arg.NotNull(scheme);
        Ensure.Arg.NotNull(configure);

        builder.AddOAuthCore();

        IServiceCollection services = builder.Services;

        services.TryAddEnumerable(
            new[]
            {
                ServiceDescriptor
                    .Transient<IOAuthOptionsResolver,
                        OAuthOptionsResolver<OAuthClientOptions>>(),

                ServiceDescriptor
                    .Transient<IValidateOptions<OAuthClientOptions>,
                        OAuthClientOptionsValidator>(),
            });

        return builder.AddScheme<
            OAuthClientOptions,
            OAuthParameters,
            OAuthClientHandler>(scheme, configure);
    }

    /// <summary>
    /// Adds OAuth client credentials authentication with default scheme.
    /// </summary>
    /// <param name="builder">The <see cref="IHttpClientAuthenticationBuilder"/>.</param>
    /// <param name="configure">A delegate that configures the <see cref="OAuthClientOptions"/>.</param>
    /// <returns>The <see cref="IHttpClientAuthenticationBuilder"/> to allow chaining the calls.</returns>
    public static IHttpClientAuthenticationBuilder AddOAuthClient(
        this IHttpClientAuthenticationBuilder builder,
        Action<OAuthClientOptions> configure)
    {
        return builder.AddOAuthClient(OAuthClientDefaults.AuthenticationScheme, configure);
    }

    /// <summary>
    /// Adds OAuth password authentication scheme.
    /// </summary>
    /// <param name="builder">The <see cref="IHttpClientAuthenticationBuilder"/>.</param>
    /// <param name="scheme">The name of the client authentication scheme.</param>
    /// <param name="configure">A delegate that configures the <see cref="OAuthPasswordOptions"/>.</param>
    /// <returns>The <see cref="IHttpClientAuthenticationBuilder"/> to allow chaining the calls.</returns>
    public static IHttpClientAuthenticationBuilder AddOAuthPassword(
        this IHttpClientAuthenticationBuilder builder,
        string scheme,
        Action<OAuthPasswordOptions> configure)
    {
        Ensure.Arg.NotNull(scheme);
        Ensure.Arg.NotNull(configure);

        builder.AddOAuthCore();

        IServiceCollection services = builder.Services;

        services.TryAddEnumerable(
            new[]
            {
                ServiceDescriptor
                    .Transient<IOAuthOptionsResolver,
                        OAuthOptionsResolver<OAuthPasswordOptions>>(),

                ServiceDescriptor
                    .Transient<IValidateOptions<OAuthPasswordOptions>,
                        OAuthPasswordOptionsValidator>(),
            });

        return builder.AddScheme<
            OAuthPasswordOptions,
            OAuthParameters,
            OAuthPasswordHandler>(scheme, configure);
    }

    /// <summary>
    /// Adds OAuth password authentication with default scheme.
    /// </summary>
    /// <param name="builder">The <see cref="IHttpClientAuthenticationBuilder"/>.</param>
    /// <param name="configure">A delegate that configures the <see cref="OAuthPasswordOptions"/>.</param>
    /// <returns>The <see cref="IHttpClientAuthenticationBuilder"/> to allow chaining the calls.</returns>
    public static IHttpClientAuthenticationBuilder AddOAuthPassword(
        this IHttpClientAuthenticationBuilder builder,
        Action<OAuthPasswordOptions> configure)
    {
        return builder.AddOAuthPassword(OAuthClientDefaults.AuthenticationScheme, configure);
    }

    /// <summary>
    /// Configures the OAuth token client.
    /// </summary>
    /// <param name="builder">The <see cref="IHttpClientAuthenticationBuilder"/>.</param>
    /// <param name="configure">A delegate that configures the <see cref="IHttpClientBuilder"/>.</param>
    /// <returns>The <see cref="IHttpClientAuthenticationBuilder"/> to allow chaining the calls.</returns>
    public static IHttpClientAuthenticationBuilder ConfigureOAuthTokenClient(
        this IHttpClientAuthenticationBuilder builder,
        Action<IHttpClientBuilder> configure)
    {
        Ensure.Arg.NotNull(builder);
        Ensure.Arg.NotNull(configure);

        IServiceCollection services = builder.Services;

        configure(services.AddHttpClient<IOAuthTokenClient, OAuthTokenClient>());
        return builder;
    }
}