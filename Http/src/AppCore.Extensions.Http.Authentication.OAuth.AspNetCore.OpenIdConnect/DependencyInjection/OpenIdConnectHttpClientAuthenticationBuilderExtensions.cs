// Licensed under the MIT License.
// Copyright (c) 2018-2022 the AppCore .NET project.

using System;
using AppCore.Diagnostics;
using AppCore.Extensions.Http.Authentication.OAuth;
using AppCore.Extensions.Http.Authentication.OAuth.AspNetCore.OpenIdConnect;
using Microsoft.Extensions.DependencyInjection.Extensions;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

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
    /// <param name="configure"></param>
    /// <returns></returns>
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
    /// <param name="configure"></param>
    /// <returns></returns>
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
            ServiceDescriptor.Scoped<OpenIdConnectUserTokenStore, OpenIdConnectUserTokenStore>()
        });

        return builder;
    }

    /// <summary>
    /// Adds OAuth user authentication by using authentication tokens from a ASP.NET Core
    /// OpenID connect authentication scheme.
    /// </summary>
    /// <param name="builder">The <see cref="IHttpClientAuthenticationBuilder"/>.</param>
    /// <param name="configure"></param>
    /// <returns></returns>
    public static IHttpClientAuthenticationBuilder AddOpenIdConnect(
        this IHttpClientAuthenticationBuilder builder,
        Action<OpenIdConnectUserOptions>? configure = null)
    {
        return AddOpenIdConnect(builder, OpenIdConnectUserDefaults.AuthenticationScheme, configure);
    }
}
