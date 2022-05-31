// Licensed under the MIT License.
// Copyright (c) 2018-2022 the AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppCore.Diagnostics;
using Microsoft.Extensions.Options;

namespace AppCore.Extensions.Http.Authentication;

/// <summary>
/// Default implementation of <see cref="IAuthenticationSchemeProvider"/>.
/// </summary>
public class AuthenticationSchemeProvider : IAuthenticationSchemeProvider, IDisposable
{
    private readonly IDisposable _optionsListener;
    private readonly Dictionary<string, AuthenticationScheme> _schemes = new(StringComparer.Ordinal);

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthenticationSchemeProvider"/> class.
    /// </summary>
    /// <param name="optionsMonitor"></param>
    public AuthenticationSchemeProvider(IOptionsMonitor<HttpClientAuthenticationOptions> optionsMonitor)
    {
        Ensure.Arg.NotNull(optionsMonitor);

        _optionsListener = optionsMonitor.OnChange(ApplyOptions);
        ApplyOptions(optionsMonitor.CurrentValue);
    }

    private void ApplyOptions(HttpClientAuthenticationOptions o)
    {
        foreach (AuthenticationScheme scheme in o.SchemeMap.Values)
        {
            _schemes[scheme.Name] = scheme;
        }
    }

    /// <inheritdoc />
    public void AddScheme(AuthenticationScheme scheme)
    {
        Ensure.Arg.NotNull(scheme);
        _schemes.Add(scheme.Name, scheme);
    }

    /// <inheritdoc />
    public Task<IReadOnlyCollection<AuthenticationScheme>> GetAllSchemesAsync()
    {
        return Task.FromResult<IReadOnlyCollection<AuthenticationScheme>>(_schemes.Values.ToArray());
    }

    /// <inheritdoc />
    public void RemoveScheme(string name)
    {
        Ensure.Arg.NotNull(name);
        _schemes.Remove(name);
    }

    /// <inheritdoc />
    public Task<AuthenticationScheme?> FindSchemeAsync(string name)
    {
        Ensure.Arg.NotNull(name);
        _schemes.TryGetValue(name, out AuthenticationScheme? scheme);
        return Task.FromResult<AuthenticationScheme?>(scheme);
    }

    /// <summary>
    /// Releases resources.
    /// </summary>
    /// <param name="disposing"><c>true</c> to dispose unmanaged and managed resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _optionsListener.Dispose();
        }
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}