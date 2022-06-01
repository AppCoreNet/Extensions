// Licensed under the MIT License.
// Copyright (c) 2018-2022 the AppCore .NET project.

using System;
using System.Linq;
using AppCore.Diagnostics;
using AppCore.Extensions.Http.Authentication.OAuth;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Provides extension methods to register OAuth authentication.
/// </summary>
public static class OAuthHttpClientAuthenticationBuilderExtensions
{
    private static void AddOAuthCore(this IHttpClientAuthenticationBuilder builder)
    {
        IServiceCollection services = builder.Services;

        services.AddDistributedMemoryCache();

        services.AddOptions<OAuthTokenCacheOptions>();
        services.TryAddTransient<IOAuthAuthenticationOptionsProvider, OAuthAuthenticationOptionsProvider>();
        services.TryAddTransient<IOAuthTokenCache, OAuthTokenCache>();
        services.TryAddTransient<IOAuthTokenService, OAuthTokenService>();
        services.AddHttpClient<IOAuthTokenClient, OAuthTokenClient>();
    }

    /// <summary>
    /// Adds OAuth client credentials authentication scheme.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="name"></param>
    /// <param name="configure"></param>
    /// <returns></returns>
    public static IHttpClientAuthenticationBuilder AddOAuthClient(
        this IHttpClientAuthenticationBuilder builder,
        string name,
        Action<OAuthClientAuthenticationOptions> configure)
    {
        Ensure.Arg.NotNull(builder);
        Ensure.Arg.NotNull(name);
        Ensure.Arg.NotNull(configure);

        IServiceCollection services = builder.Services;

        builder.AddOAuthCore();

        services.TryAddEnumerable(
            ServiceDescriptor
                .Transient<IValidateOptions<OAuthClientAuthenticationOptions>, OAuthClientAuthenticationOptionsValidator>());

        return builder.AddScheme<
            OAuthClientAuthenticationOptions,
            OAuthAuthenticationParameters,
            OAuthClientAuthenticationHandler>(name, configure);
    }

    /// <summary>
    /// Adds OAuth password authentication scheme.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="name"></param>
    /// <param name="configure"></param>
    /// <returns></returns>
    public static IHttpClientAuthenticationBuilder AddOAuthPassword(
        this IHttpClientAuthenticationBuilder builder,
        string name,
        Action<OAuthPasswordAuthenticationOptions> configure)
    {
        Ensure.Arg.NotNull(builder);
        Ensure.Arg.NotNull(name);
        Ensure.Arg.NotNull(configure);

        IServiceCollection services = builder.Services;

        builder.AddOAuthCore();

        services.TryAddEnumerable(
            ServiceDescriptor
                .Transient<IValidateOptions<OAuthPasswordAuthenticationOptions>, OAuthPasswordAuthenticationOptionsValidator>());

        return builder.AddScheme<
            OAuthPasswordAuthenticationOptions,
            OAuthAuthenticationParameters,
            OAuthPasswordAuthenticationHandler>(name, configure);
    }

    /// <summary>
    /// Configures the OAuth token client.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="configure"></param>
    /// <returns></returns>
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