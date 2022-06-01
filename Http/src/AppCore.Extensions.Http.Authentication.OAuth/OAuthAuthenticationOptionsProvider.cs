// Licensed under the MIT License.
// Copyright (c) 2018-2022 the AppCore .NET project.

using System;
using System.Threading.Tasks;
using AppCore.Diagnostics;
using Microsoft.Extensions.Options;

namespace AppCore.Extensions.Http.Authentication.OAuth;

/// <summary>
/// Default implementation of the <see cref="IOAuthAuthenticationOptionsProvider"/>.
/// </summary>
public sealed class OAuthAuthenticationOptionsProvider : IOAuthAuthenticationOptionsProvider
{
    private readonly IOptionsMonitor<OAuthClientAuthenticationOptions> _clientOptionsMonitor;
    private readonly IOptionsMonitor<OAuthPasswordAuthenticationOptions> _passwordOptionsMonitor;

    /// <summary>
    /// Initializes a new instance of the <see cref="OAuthAuthenticationOptionsProvider"/> class.
    /// </summary>
    /// <param name="clientOptionsMonitor"></param>
    /// <param name="passwordOptionsMonitor"></param>
    public OAuthAuthenticationOptionsProvider(
        IOptionsMonitor<OAuthClientAuthenticationOptions> clientOptionsMonitor,
        IOptionsMonitor<OAuthPasswordAuthenticationOptions> passwordOptionsMonitor)
    {
        Ensure.Arg.NotNull(clientOptionsMonitor);
        Ensure.Arg.NotNull(passwordOptionsMonitor);

        _clientOptionsMonitor = clientOptionsMonitor;
        _passwordOptionsMonitor = passwordOptionsMonitor;
    }

    /// <inheritdoc />
    public Task<T> GetOptionsAsync<T>(AuthenticationScheme scheme)
        where T : OAuthAuthenticationOptions
    {
        Ensure.Arg.NotNull(scheme);

        if (typeof(T) == typeof(OAuthClientAuthenticationOptions))
        {
            scheme.EnsureClientScheme();
            return Task.FromResult((T)(object)_clientOptionsMonitor.Get(scheme.Name));
        }

        if (typeof(T) == typeof(OAuthPasswordAuthenticationOptions))
        {
            scheme.EnsurePasswordScheme();
            return Task.FromResult((T)(object)_passwordOptionsMonitor.Get(scheme.Name));
        }

        throw new ArgumentException($"The options type {typeof(T)} is not handled.");
    }
}