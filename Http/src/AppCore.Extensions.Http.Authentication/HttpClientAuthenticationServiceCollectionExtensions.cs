// Licensed under the MIT License.
// Copyright (c) 2018-2022 the AppCore .NET project.

using AppCore.Extensions.Http.Authentication;
using Microsoft.Extensions.DependencyInjection.Extensions;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Extension methods for <see cref="IServiceCollection"/> to register HTTP client authentication services.
/// </summary>
public static class HttpClientAuthenticationServiceCollectionExtensions
{
    /// <summary>
    /// Adds the token management services to DI using all default values
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/>.</param>
    /// <returns></returns>
    public static IHttpClientAuthenticationBuilder AddHttpClientAuthentication(this IServiceCollection services)
    {
        services.TryAddSingleton<IAuthenticationSchemeProvider, AuthenticationSchemeProvider>();
        return new HttpClientAuthenticationBuilder(services);
    }
}