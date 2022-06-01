// Licensed under the MIT License.
// Copyright (c) 2018-2022 the AppCore .NET project.

using AppCore.Diagnostics;
using AppCore.Extensions.Http.Authentication;
using Microsoft.Extensions.Logging;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Provides extension methods to register authentication with a <see cref="IHttpClientBuilder"/>.
/// </summary>
public static class AuthenticationHttpClientBuilderExtensions
{
    private static string GetLoggerName(IHttpClientBuilder builder)
    {
        return $"System.Net.Http.HttpClient.{builder.Name}.AuthenticationHandler";
    }

    /// <summary>
    /// Adds authentication to the specified <see cref="IHttpClientBuilder"/>.
    /// </summary>
    /// <param name="builder">The <see cref="IHttpClientBuilder"/>.</param>
    /// <param name="scheme">The authentication scheme.</param>
    /// <param name="parameters">Additional authentication parameters.</param>
    /// <typeparam name="TParameters">The type of the <see cref="AuthenticationParameters"/>.</typeparam>
    /// <typeparam name="THandler">The type of the <see cref="IAuthenticationSchemeHandler{TParameters}"/>.</typeparam>
    /// <returns>The passed <see cref="IHttpClientBuilder"/>.</returns>
    public static IHttpClientBuilder AddAuthentication<TParameters, THandler>(
        this IHttpClientBuilder builder,
        string scheme,
        TParameters? parameters)
        where TParameters : AuthenticationParameters, new()
        where THandler : IAuthenticationSchemeHandler<TParameters>
    {
        Ensure.Arg.NotNull(builder);
        Ensure.Arg.NotNull(scheme);

        string loggerName = GetLoggerName(builder);

        return builder.AddHttpMessageHandler(
            sp =>
                new AuthenticationHandler<TParameters, THandler>(
                    scheme,
                    sp.GetRequiredService<IAuthenticationSchemeProvider>(),
                    sp.GetRequiredService<THandler>(),
                    parameters,
                    sp.GetRequiredService<ILoggerFactory>()
                      .CreateLogger(loggerName)));
    }
}