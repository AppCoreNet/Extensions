// Licensed under the MIT License.
// Copyright (c) 2018-2022 the AppCore .NET project.

using System;
using AppCore.Diagnostics;
using AppCore.Extensions.Http.Authentication.OAuth;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Provides extension methods to register OAuth authentication.
/// </summary>
public static class OAuthHttpClientAuthenticationBuilderExtensions
{
    /// <summary>
    /// Adds OAuth client authentication services.
    /// </summary>
    /// <param name="builder">The <see cref="IHttpClientAuthenticationBuilder"/>.</param>
    /// <returns></returns>
    public static IHttpClientAuthenticationBuilder AddOAuthCore(this IHttpClientAuthenticationBuilder builder)
    {
        Ensure.Arg.NotNull(builder);

        IServiceCollection services = builder.Services;

        services.AddDistributedMemoryCache();

        services.AddOptions<OAuthTokenCacheOptions>();
        services.TryAddSingleton<IOAuthTokenCache, OAuthTokenCache>();
        services.TryAddSingleton<IOAuthTokenService, OAuthTokenService>();

        services.TryAddTransient<IOAuthOptionsProvider, OAuthOptionsProvider>();
        services.AddHttpClient<IOAuthTokenClient, OAuthTokenClient>();

        return builder;
    }

    /// <summary>
    /// Adds OAuth client credentials authentication scheme.
    /// </summary>
    /// <param name="builder">The <see cref="IHttpClientAuthenticationBuilder"/>.</param>
    /// <param name="scheme">The name of the client authentication scheme.</param>
    /// <param name="configure"></param>
    /// <returns></returns>
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
                        OAuthClientOptionsValidator>()
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
    /// <param name="configure"></param>
    /// <returns></returns>
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
    /// <param name="configure"></param>
    /// <returns></returns>
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
                        OAuthPasswordOptionsValidator>()
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
    /// <param name="configure"></param>
    /// <returns></returns>
    public static IHttpClientAuthenticationBuilder AddOAuthPassword(
        this IHttpClientAuthenticationBuilder builder,
        Action<OAuthPasswordOptions> configure)
    {
        return builder.AddOAuthPassword(OAuthClientDefaults.AuthenticationScheme, configure);
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