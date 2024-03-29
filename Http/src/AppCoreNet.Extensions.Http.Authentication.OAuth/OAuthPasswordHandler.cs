﻿using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using AppCoreNet.Diagnostics;

namespace AppCoreNet.Extensions.Http.Authentication.OAuth;

/// <summary>
/// Provides a OAuth password authentication handler.
/// </summary>
public class OAuthPasswordHandler : IAuthenticationSchemeHandler<OAuthParameters>
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

    /// <inheritdoc />
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
}