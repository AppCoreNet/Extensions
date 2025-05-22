// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AppCoreNet.Extensions.Http.Authentication;

/// <summary>
/// Represents a HTTP client authentication scheme handler.
/// </summary>
public interface IAuthenticationSchemeHandler
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
        AuthenticationParameters? parameters = null,
        CancellationToken cancellationToken = default);
}