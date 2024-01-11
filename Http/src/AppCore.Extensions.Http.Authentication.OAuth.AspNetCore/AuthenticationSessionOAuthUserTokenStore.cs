// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System.Collections.Generic;
using System.Security.Authentication;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AppCoreNet.Diagnostics;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AppCore.Extensions.Http.Authentication.OAuth.AspNetCore;

/// <summary>
/// Provides the base class for <see cref="IOAuthUserTokenStore"/> which stores tokens in the authentication
/// session.
/// </summary>
/// <typeparam name="TOptions">The type of the <see cref="OAuthUserOptions"/>.</typeparam>
public abstract class AuthenticationSessionOAuthUserTokenStore<TOptions> : IOAuthUserTokenStore
    where TOptions : OAuthUserOptions
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IOptionsMonitor<TOptions> _optionsMonitor;
    private readonly ILogger _logger;

    // per-request cache so that if SignInAsync is used, we won't re-read the old/cached AuthenticateResult from the handler
    // this requires this service to be added as scoped to the DI system
    private readonly Dictionary<string, AuthenticateResult> _cache = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthenticationSessionOAuthUserTokenStore{TOptions}"/> class.
    /// </summary>
    /// <param name="httpContextAccessor">The <see cref="IHttpContextAccessor"/>.</param>
    /// <param name="optionsMonitor">The <see cref="IOptionsMonitor{TOptions}"/>.</param>
    /// <param name="logger">The <see cref="ILogger{TCategoryName}"/>.</param>
    protected AuthenticationSessionOAuthUserTokenStore(
        IHttpContextAccessor httpContextAccessor,
        IOptionsMonitor<TOptions> optionsMonitor,
        ILogger logger)
    {
        Ensure.Arg.NotNull(httpContextAccessor);
        Ensure.Arg.NotNull(optionsMonitor);
        Ensure.Arg.NotNull(logger);

        _httpContextAccessor = httpContextAccessor;
        _optionsMonitor = optionsMonitor;
        _logger = logger;
    }

    private HttpContext GetHttpContext()
    {
        HttpContext? httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null)
            throw new AuthenticationException("No HttpContext found.");

        return httpContext;
    }

    private async Task<string> GetSignInScheme(HttpContext context, TOptions options)
    {
        string? scheme = string.IsNullOrWhiteSpace(options.SignInScheme)
            ? options.SignInScheme
            : null;

        if (scheme == null)
        {
            var schemeProvider = context.RequestServices
                                        .GetRequiredService<Microsoft.AspNetCore.Authentication.
                                            IAuthenticationSchemeProvider>();

            scheme = (await schemeProvider.GetDefaultSignInSchemeAsync())?.Name;
        }

        if (scheme == null)
            throw new AuthenticationException("There is no default sign-in scheme configured for ASP.NET Core authentication.");

        return scheme;
    }

    private async Task<AuthenticateResult?> TryAuthenticateAsync(HttpContext httpContext, string signInScheme)
    {
        // check the cache in case the token was re-issued via StoreTokenAsync
        if (!_cache.TryGetValue(signInScheme, out AuthenticateResult? result))
        {
            result = await httpContext.AuthenticateAsync(signInScheme);
        }

        if (!result.Succeeded)
        {
            _logger.LogError("Cannot authenticate scheme: {schemeName}", signInScheme);
            return null;
        }

        if (result.Properties == null)
        {
            _logger.LogError("Authentication result properties are null for scheme: {schemeName}",
                             signInScheme);

            return null;
        }

        return result;
    }

    /// <summary>
    /// Ensures that the <paramref name="scheme"/> is compatible.
    /// </summary>
    /// <param name="scheme">The <see cref="AuthenticationScheme"/>.</param>
    protected abstract void EnsureCompatibleScheme(AuthenticationScheme scheme);

    /// <inheritdoc />
    public async Task StoreTokenAsync(
        AuthenticationScheme scheme,
        ClaimsPrincipal user,
        OAuthUserToken token,
        CancellationToken cancellationToken = default)
    {
        Ensure.Arg.NotNull(scheme);
        Ensure.Arg.NotNull(user);
        Ensure.Arg.NotNull(token);

        EnsureCompatibleScheme(scheme);

        HttpContext httpContext = GetHttpContext();
        TOptions options = _optionsMonitor.Get(scheme.Name);
        string signInScheme = await GetSignInScheme(httpContext, options);

        AuthenticateResult? result = await TryAuthenticateAsync(httpContext, signInScheme);
        if (result == null)
            throw new AuthenticationException("User is not authenticated, cannot store tokens.");

        ClaimsPrincipal principal = result.Principal!;
        StoreToken(principal, result.Properties!, token, options);

        if (result.Properties!.AllowRefresh.GetValueOrDefault(true))
        {
            result.Properties.IssuedUtc = null;
            result.Properties.ExpiresUtc = null;
        }

        await httpContext.SignInAsync(signInScheme, principal, result.Properties);
        _cache[signInScheme] = AuthenticateResult.Success(new AuthenticationTicket(principal, result.Properties, signInScheme));
    }

    /// <summary>
    /// Stores the token in the authentication session.
    /// </summary>
    /// <param name="principal"></param>
    /// <param name="properties"></param>
    /// <param name="token"></param>
    /// <param name="options"></param>
    protected abstract void StoreToken(
        ClaimsPrincipal principal,
        AuthenticationProperties properties,
        OAuthUserToken token,
        TOptions options);

    /// <inheritdoc />
    public async Task<OAuthUserToken> GetTokenAsync(
        AuthenticationScheme scheme,
        ClaimsPrincipal user,
        CancellationToken cancellationToken = default)
    {
        Ensure.Arg.NotNull(scheme);
        Ensure.Arg.NotNull(user);

        EnsureCompatibleScheme(scheme);

        HttpContext httpContext = GetHttpContext();
        TOptions options = _optionsMonitor.Get(scheme.Name);
        string signInScheme = await GetSignInScheme(httpContext, options);

        AuthenticateResult? result = await TryAuthenticateAsync(httpContext, signInScheme);
        if (result == null)
            throw new AuthenticationException("User is not authenticated, cannot get tokens.");

        return GetToken(result.Principal!, result.Properties!, options);
    }

    /// <summary>
    /// Gets the token from the authentication session.
    /// </summary>
    /// <param name="principal"></param>
    /// <param name="properties"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    protected abstract OAuthUserToken GetToken(
        ClaimsPrincipal principal,
        AuthenticationProperties properties,
        TOptions options);

    /// <inheritdoc />
    public Task ClearTokenAsync(
        AuthenticationScheme scheme,
        ClaimsPrincipal user,
        CancellationToken cancellationToken = default)
    {
        Ensure.Arg.NotNull(scheme);
        Ensure.Arg.NotNull(user);

        EnsureCompatibleScheme(scheme);

        return Task.CompletedTask;
    }
}