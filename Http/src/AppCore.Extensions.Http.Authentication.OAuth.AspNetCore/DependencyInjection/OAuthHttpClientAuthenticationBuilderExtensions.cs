// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using AppCoreNet.Diagnostics;

using AppCore.Extensions.Http.Authentication.OAuth;

// ReSharper disable once CheckNamespace
namespace AppCore.Extensions.DependencyInjection;

/// <summary>
/// Provides extension methods to register OAuth authentication.
/// </summary>
public static class OAuthHttpClientAuthenticationBuilderExtensions
{
    /// <summary>
    /// Adds OAuth client credentials authentication scheme by inferring the configuration from a ASP.NET Core
    /// authentication scheme.
    /// </summary>
    /// <param name="builder">The <see cref="IHttpClientAuthenticationBuilder"/>.</param>
    /// <param name="scheme">The name of the client authentication scheme.</param>
    /// <param name="configure"></param>
    /// <returns></returns>
    public static IHttpClientAuthenticationBuilder AddOAuthClientForScheme(
        this IHttpClientAuthenticationBuilder builder,
        string scheme,
        Action<IOAuthClientFromAuthenticationSchemeBuilder> configure)
    {
        Ensure.Arg.NotNull(scheme);
        Ensure.Arg.NotNull(configure);

        builder.AddOAuthCore();
        configure(new OAuthClientFromAuthenticationSchemeBuilder(builder.Services, scheme));

        return builder;
    }

    /// <summary>
    /// Adds OAuth client credentials authentication scheme by inferring the configuration from a ASP.NET Core
    /// authentication scheme.
    /// </summary>
    /// <param name="builder">The <see cref="IHttpClientAuthenticationBuilder"/>.</param>
    /// <param name="configure"></param>
    /// <returns></returns>
    public static IHttpClientAuthenticationBuilder AddOAuthClientForScheme(
        this IHttpClientAuthenticationBuilder builder,
        Action<IOAuthClientFromAuthenticationSchemeBuilder> configure)
    {
        return builder.AddOAuthClientForScheme(OAuthClientDefaults.AuthenticationScheme, configure);
    }
}