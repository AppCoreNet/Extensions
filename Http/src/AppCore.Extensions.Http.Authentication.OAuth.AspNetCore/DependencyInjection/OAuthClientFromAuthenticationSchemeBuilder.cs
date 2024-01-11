// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using AppCoreNet.Diagnostics;

using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace AppCore.Extensions.DependencyInjection;

internal sealed class OAuthClientFromAuthenticationSchemeBuilder : IOAuthClientFromAuthenticationSchemeBuilder
{
    public IServiceCollection Services { get; }

    public string Scheme { get; }

    public OAuthClientFromAuthenticationSchemeBuilder(IServiceCollection services, string scheme)
    {
        Ensure.Arg.NotNull(services);
        Ensure.Arg.NotNull(scheme);

        Services = services;
        Scheme = scheme;
    }
}