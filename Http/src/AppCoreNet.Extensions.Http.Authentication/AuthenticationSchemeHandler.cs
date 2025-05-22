// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AppCoreNet.Diagnostics;
using Microsoft.Extensions.Options;

namespace AppCoreNet.Extensions.Http.Authentication;

/// <summary>
/// Provides a base class for <see cref="IAuthenticationSchemeHandler"/>.
/// </summary>
/// <typeparam name="TOptions">The type of the <see cref="AuthenticationSchemeOptions"/>.</typeparam>
/// <typeparam name="TParameters">The type of the <see cref="AuthenticationParameters"/>.</typeparam>
public abstract class AuthenticationSchemeHandler<
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TOptions, TParameters>
    : IAuthenticationSchemeHandler
    where TOptions : AuthenticationSchemeOptions
    where TParameters : AuthenticationParameters, new()
{
    private readonly IOptionsMonitor<TOptions> _optionsMonitor;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthenticationSchemeHandler{TOptions,TParameters}"/> class.
    /// </summary>
    /// <param name="optionsMonitor">The <see cref="IOptionsMonitor{TOptions}"/>.</param>
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

    async Task IAuthenticationSchemeHandler.AuthenticateAsync(
        AuthenticationScheme scheme,
        HttpRequestMessage request,
        AuthenticationParameters? parameters,
        CancellationToken cancellationToken)
    {
        await AuthenticateAsync(
                scheme,
                _optionsMonitor.Get(scheme.Name),
                (TParameters?)parameters,
                request,
                cancellationToken)
            .ConfigureAwait(false);
    }
}