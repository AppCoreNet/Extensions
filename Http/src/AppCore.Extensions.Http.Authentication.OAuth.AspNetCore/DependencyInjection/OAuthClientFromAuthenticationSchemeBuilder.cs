// Licensed under the MIT License.
// Copyright (c) 2018-2022 the AppCore .NET project.

using AppCore.Diagnostics;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

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