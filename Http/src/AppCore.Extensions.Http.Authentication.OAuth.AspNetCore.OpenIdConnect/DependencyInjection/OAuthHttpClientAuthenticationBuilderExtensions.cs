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
public static class OAuthHttpClientAuthenticationBuilderExtensions
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
        Action<OpenIdConnectOAuthClientOptions>? configure = null)
    {
        Ensure.Arg.NotNull(builder);

        IServiceCollection services = builder.Services;

        services.AddHttpClientAuthentication()
                .AddScheme<
                    OpenIdConnectOAuthClientOptions,
                    OAuthParameters,
                    OAuthClientHandler>(builder.Scheme);

        services.TryAddEnumerable(
            new[]
            {
                ServiceDescriptor
                    .Transient<IOAuthOptionsResolver,
                        OpenIdConnectOAuthClientOptionsResolver>(),
            });

        if (configure != null)
        {
            services.Configure(builder.Scheme, configure);
        }
    }
}