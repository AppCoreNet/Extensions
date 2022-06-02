// Licensed under the MIT License.
// Copyright (c) 2018-2022 the AppCore .NET project.

using System;
using AppCore.Diagnostics;
using AppCore.Extensions.Http.Authentication.OAuth;
using AppCore.Extensions.Http.Authentication.OAuth.AspNetCore;
using Microsoft.Extensions.DependencyInjection.Extensions;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Provides extension methods to register OAuth authentication.
/// </summary>
public static class OAuthHttpClientAuthenticationBuilderExtensions
{
    /// <summary>
    /// Adds OAuth client credentials authentication scheme by inferring the configuration from a OpenID connect
    /// authentication scheme.
    /// </summary>
    /// <param name="builder">The <see cref="IHttpClientAuthenticationBuilder"/>.</param>
    /// <param name="scheme">The name of the client authentication scheme.</param>
    /// <param name="configure"></param>
    /// <returns></returns>
    public static IHttpClientAuthenticationBuilder AddOAuthClientFromOpenIdConnect(
        this IHttpClientAuthenticationBuilder builder,
        string scheme,
        Action<OpenIdConnectOAuthClientOptions>? configure = null)
    {
        Ensure.Arg.NotNull(scheme);

        builder.AddOAuthCore();

        IServiceCollection services = builder.Services;

        services.TryAddEnumerable(
            new[]
            {
                ServiceDescriptor
                    .Transient<IOAuthOptionsResolver,
                        OpenIdConnectOAuthClientOptionsResolver>(),
            });

        if (configure != null)
        {
            services.Configure(scheme, configure);
        }

        return builder.AddScheme<
            OAuthClientOptions,
            OAuthParameters,
            OAuthClientHandler>(scheme);
    }

    /// <summary>
    /// Adds OAuth client credentials authentication with default scheme name by inferring the configuration from
    /// a OpenID connect authentication scheme.
    /// </summary>
    /// <param name="builder">The <see cref="IHttpClientAuthenticationBuilder"/>.</param>
    /// <param name="configure"></param>
    /// <returns></returns>
    public static IHttpClientAuthenticationBuilder AddOAuthClientFromOpenIdConnect(
        this IHttpClientAuthenticationBuilder builder,
        Action<OpenIdConnectOAuthClientOptions>? configure = null)
    {
        return builder.AddOAuthClientFromOpenIdConnect(OAuthClientDefaults.AuthenticationScheme, configure);
    }
}