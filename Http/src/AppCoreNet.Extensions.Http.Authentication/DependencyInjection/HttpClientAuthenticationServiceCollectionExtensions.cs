// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using AppCoreNet.Extensions.Http.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

// ReSharper disable once CheckNamespace
namespace AppCoreNet.Extensions.DependencyInjection;

/// <summary>
/// Extension methods for <see cref="IServiceCollection"/> to register HTTP client authentication services.
/// </summary>
public static class HttpClientAuthenticationServiceCollectionExtensions
{
    /// <summary>
    /// Adds the token management services to DI using all default values.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <returns>The <see cref="IHttpClientAuthenticationBuilder"/> to configure the authentication.</returns>
    public static IHttpClientAuthenticationBuilder AddHttpClientAuthentication(this IServiceCollection services)
    {
        services.TryAddSingleton<IAuthenticationSchemeProvider, AuthenticationSchemeProvider>();
        return new HttpClientAuthenticationBuilder(services);
    }
}