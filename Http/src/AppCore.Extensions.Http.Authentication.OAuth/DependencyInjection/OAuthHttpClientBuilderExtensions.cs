// Licensed under the MIT License.
// Copyright (c) 2018-2022 the AppCore .NET project.

using AppCore.Diagnostics;
using AppCore.Extensions.Http.Authentication.OAuth;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Provides extension methods to add OAuth authentication to a HttpClient.
/// </summary>
public static class OAuthHttpClientBuilderExtensions
{
    /// <summary>
    /// Adds OAuth client credentials authentications.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="scheme"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public static IHttpClientBuilder AddOAuthClientAuthentication(
        this IHttpClientBuilder builder,
        string scheme,
        OAuthAuthenticationParameters? parameters = null)
    {
        Ensure.Arg.NotNull(builder);
        return builder.AddAuthentication<OAuthAuthenticationParameters, OAuthClientAuthenticationHandler>(scheme, parameters);
    }

    /// <summary>
    /// Adds OAuth password authentications.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="scheme"></param>
    /// <param name="parameters"></param>
    /// <returns></returns>
    public static IHttpClientBuilder AddOAuthPasswordAuthentication(
        this IHttpClientBuilder builder,
        string scheme,
        OAuthAuthenticationParameters? parameters = null)
    {
        Ensure.Arg.NotNull(builder);
        return builder.AddAuthentication<OAuthAuthenticationParameters, OAuthPasswordAuthenticationHandler>(scheme, parameters);
    }
}