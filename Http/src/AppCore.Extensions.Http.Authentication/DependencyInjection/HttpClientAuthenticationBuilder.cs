// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using AppCoreNet.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace AppCore.Extensions.DependencyInjection;

internal sealed class HttpClientAuthenticationBuilder : IHttpClientAuthenticationBuilder
{
    public IServiceCollection Services { get; }

    public HttpClientAuthenticationBuilder(IServiceCollection services)
    {
        Ensure.Arg.NotNull(services);
        Services = services;
    }
}