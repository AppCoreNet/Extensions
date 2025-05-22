// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using AppCoreNet.Diagnostics;

namespace AppCoreNet.Extensions.Http.Authentication.OAuth;

/// <summary>
/// Provides a OAuth password authentication handler.
/// </summary>
public class OAuthPasswordHandler : IAuthenticationSchemeHandler
{
    private readonly IOAuthTokenService _authTokenService;

    /// <summary>
    /// Initializes a new instance of the <see cref="OAuthPasswordHandler"/> class.
    /// </summary>
    /// <param name="authTokenService">The <see cref="IOAuthTokenService"/>.</param>
    public OAuthPasswordHandler(IOAuthTokenService authTokenService)
    {
        Ensure.Arg.NotNull(authTokenService);
        _authTokenService = authTokenService;
    }

    /// <summary>
    /// Authenticates a <see cref="HttpRequestMessage"/> with the specified OAuth password scheme and parameters.
    /// </summary>
    /// <param name="scheme">The <see cref="AuthenticationScheme"/>.</param>
    /// <param name="request">The <see cref="HttpRequestMessage"/>.</param>
    /// <param name="parameters">The <see cref="OAuthParameters"/>.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>The asynchronous operation.</returns>
    public async Task AuthenticateAsync(
        AuthenticationScheme scheme,
        HttpRequestMessage request,
        OAuthParameters? parameters,
        CancellationToken cancellationToken = default)
    {
        OAuthAccessToken accessToken =
            await _authTokenService.GetPasswordAccessTokenAsync(scheme, parameters, cancellationToken)
                                   .ConfigureAwait(false);

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.AccessToken);
    }

    async Task IAuthenticationSchemeHandler.AuthenticateAsync(
        AuthenticationScheme scheme,
        HttpRequestMessage request,
        AuthenticationParameters? parameters,
        CancellationToken cancellationToken)
    {
        await AuthenticateAsync(
                scheme,
                request,
                (OAuthParameters?)parameters,
                cancellationToken)
            .ConfigureAwait(false);
    }
}