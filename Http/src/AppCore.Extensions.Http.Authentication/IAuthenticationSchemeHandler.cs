// Licensed under the MIT License.
// Copyright (c) 2018-2022 the AppCore .NET project.

using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AppCore.Extensions.Http.Authentication;

/// <summary>
/// Represents a HTTP client authentication scheme handler.
/// </summary>
/// <typeparam name="TParameters">The type of the <see cref="AuthenticationParameters"/>.</typeparam>
public interface IAuthenticationSchemeHandler<in TParameters>
    where TParameters : AuthenticationParameters
{
    /// <summary>
    /// Authenticates a <see cref="HttpRequestMessage"/> with the specified scheme and parameters.
    /// </summary>
    /// <param name="scheme">The <see cref="AuthenticationScheme"/>.</param>
    /// <param name="request">The <see cref="HttpRequestMessage"/>.</param>
    /// <param name="parameters">The <see cref="AuthenticationParameters"/>.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>The asynchronous operation.</returns>
    Task AuthenticateAsync(
        AuthenticationScheme scheme,
        HttpRequestMessage request,
        TParameters? parameters = null,
        CancellationToken cancellationToken = default);
}