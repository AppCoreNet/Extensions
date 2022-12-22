// Licensed under the MIT License.
// Copyright (c) 2018-2022 the AppCore .NET project.

using AppCore.Diagnostics;
using AppCore.Extensions.Http.Authentication.OAuth.AspNetCore.OpenIdConnect;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

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
    /// <param name="parameters"></param>
    /// <returns></returns>
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
    /// <param name="parameters"></param>
    /// <returns></returns>
    public static IHttpClientBuilder AddOpenIdConnectAuthentication(
        this IHttpClientBuilder builder,
        OpenIdConnectUserParameters? parameters = null)
    {
        return builder.AddOpenIdConnectAuthentication(OpenIdConnectUserDefaults.AuthenticationScheme, parameters);
    }
}