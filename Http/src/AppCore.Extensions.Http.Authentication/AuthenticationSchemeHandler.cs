// Licensed under the MIT License.
// Copyright (c) 2018-2022 the AppCore .NET project.

using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AppCore.Diagnostics;
using Microsoft.Extensions.Options;

namespace AppCore.Extensions.Http.Authentication;

/// <summary>
/// Provides a base class for <see cref="IAuthenticationSchemeHandler{TParameters}"/>.
/// </summary>
/// <typeparam name="TOptions">The type of the <see cref="AuthenticationSchemeOptions"/>.</typeparam>
/// <typeparam name="TParameters">The type of the <see cref="AuthenticationParameters"/>.</typeparam>
public abstract class AuthenticationSchemeHandler<TOptions, TParameters> : IAuthenticationSchemeHandler<TParameters>
    where TOptions : AuthenticationSchemeOptions
    where TParameters : AuthenticationParameters, new()
{
    private readonly IOptionsMonitor<TOptions> _optionsMonitor;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthenticationSchemeHandler{TOptions,TParameters}"/> class.
    /// </summary>
    /// <param name="optionsMonitor"></param>
    protected AuthenticationSchemeHandler(IOptionsMonitor<TOptions> optionsMonitor)
    {
        Ensure.Arg.NotNull(optionsMonitor);
        _optionsMonitor = optionsMonitor;
    }

    /// <summary>
    /// Must be overridden to authenticate a <see cref="HttpRequestMessage"/> with the specified scheme and parameters.
    /// </summary>
    /// <param name="scheme">The <see cref="AuthenticationScheme"/>.</param>
    /// <param name="options">The <see cref="AuthenticationSchemeOptions"/>.</param>
    /// <param name="parameters">The <see cref="AuthenticationParameters"/>.</param>
    /// <param name="request">The <see cref="HttpRequestMessage"/>.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>The asynchronous operation.</returns>
    protected abstract Task AuthenticateAsync(
        AuthenticationScheme scheme,
        TOptions options,
        TParameters? parameters,
        HttpRequestMessage request,
        CancellationToken cancellationToken);

    async Task IAuthenticationSchemeHandler<TParameters>.AuthenticateAsync(
        AuthenticationScheme scheme,
        TParameters? parameters,
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        await AuthenticateAsync(scheme, _optionsMonitor.Get(scheme.Name), parameters, request, cancellationToken);
    }
}