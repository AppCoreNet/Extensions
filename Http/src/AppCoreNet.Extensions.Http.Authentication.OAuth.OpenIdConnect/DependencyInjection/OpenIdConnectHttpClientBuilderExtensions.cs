﻿// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using AppCoreNet.Diagnostics;
using AppCoreNet.Extensions.Http.Authentication.OAuth.OpenIdConnect;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace AppCoreNet.Extensions.DependencyInjection;

/// <summary>
/// Provides extension methods to add OAuth authentication to a HttpClient.
/// </summary>
public static class OpenIdConnectHttpClientBuilderExtensions
{
    /// <summary>
    /// Adds OpenID connect authentications.
    /// </summary>
    /// <param name="builder">The <see cref="IHttpClientBuilder"/>.</param>
    /// <param name="scheme">The name of the client authentication scheme.</param>
    /// <param name="parameters">The <see cref="OpenIdConnectUserParameters"/>.</param>
    /// <returns>The <see cref="IHttpClientBuilder"/> to allow chaining the calls.</returns>
    public static IHttpClientBuilder AddOpenIdConnectAuthentication(
        this IHttpClientBuilder builder,
        string scheme,
        OpenIdConnectUserParameters? parameters = null)
    {
        Ensure.Arg.NotNull(builder);
        return builder.AddAuthentication<OpenIdConnectUserParameters, OpenIdConnectUserHandler>(scheme, parameters);
    }

    /// <summary>
    /// Adds OpenID connect  authentications with the default scheme.
    /// </summary>
    /// <param name="builder">The <see cref="IHttpClientBuilder"/>.</param>
    /// <param name="parameters">The <see cref="OpenIdConnectUserParameters"/>.</param>
    /// <returns>The <see cref="IHttpClientBuilder"/> to allow chaining the calls.</returns>
    public static IHttpClientBuilder AddOpenIdConnectAuthentication(
        this IHttpClientBuilder builder,
        OpenIdConnectUserParameters? parameters = null)
    {
        return builder.AddOpenIdConnectAuthentication(OpenIdConnectUserDefaults.AuthenticationScheme, parameters);
    }
}