using System.Collections.Generic;
using System.Security.Authentication;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AppCore.Diagnostics;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AppCore.Extensions.Http.Authentication.OAuth.AspNetCore;

public abstract class AuthenticationSessionOAuthUserTokenStore<TOptions> : IOAuthUserTokenStore
    where TOptions : OAuthUserOptions
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IOptionsMonitor<TOptions> _optionsMonitor;
    private readonly ILogger _logger;

    // per-request cache so that if SignInAsync is used, we won't re-read the old/cached AuthenticateResult from the handler
    // this requires this service to be added as scoped to the DI system
    private readonly Dictionary<string, AuthenticateResult> _cache = new();

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

    private async Task<AuthenticateResult?> TryAuthenticateAsync(HttpContext httpContext, string signInScheme, TOptions options)
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

    protected abstract void EnsureCompatibleScheme(AuthenticationScheme scheme);

    public async Task StoreTokenAsync(
        AuthenticationScheme scheme,
        ClaimsPrincipal user,
        OAuthUserToken token,
        OAuthUserParameters? parameters = null,
        CancellationToken cancellationToken = default)
    {
        Ensure.Arg.NotNull(scheme);
        Ensure.Arg.NotNull(user);
        Ensure.Arg.NotNull(token);

        EnsureCompatibleScheme(scheme);

        HttpContext httpContext = GetHttpContext();
        TOptions options = _optionsMonitor.Get(scheme.Name);
        string signInScheme = await GetSignInScheme(httpContext, options);

        AuthenticateResult? result = await TryAuthenticateAsync(httpContext, signInScheme, options);
        if (result == null)
            throw new AuthenticationException("User is not authenticated, cannot store tokens.");

        ClaimsPrincipal principal = result.Principal!;
        SetUserToken(principal, result.Properties!, token, options);

        if (result.Properties!.AllowRefresh.GetValueOrDefault(true))
        {
            result.Properties.IssuedUtc = null;
            result.Properties.ExpiresUtc = null;
        }

        await httpContext.SignInAsync(signInScheme, principal, result.Properties);
        _cache[signInScheme] = AuthenticateResult.Success(new AuthenticationTicket(principal, result.Properties, signInScheme));
    }

    protected abstract void SetUserToken(
        ClaimsPrincipal principal,
        AuthenticationProperties properties,
        OAuthUserToken token,
        TOptions options);

    public async Task<OAuthUserToken> GetTokenAsync(
        AuthenticationScheme scheme,
        ClaimsPrincipal user,
        OAuthUserParameters? parameters = null,
        CancellationToken cancellationToken = default)
    {
        Ensure.Arg.NotNull(scheme);
        Ensure.Arg.NotNull(user);

        EnsureCompatibleScheme(scheme);

        HttpContext httpContext = GetHttpContext();
        TOptions options = _optionsMonitor.Get(scheme.Name);
        string signInScheme = await GetSignInScheme(httpContext, options);

        AuthenticateResult? result = await TryAuthenticateAsync(httpContext, signInScheme, options);
        if (result == null)
            throw new AuthenticationException("User is not authenticated, cannot get tokens.");

        return GetUserToken(result.Principal!, result.Properties!, options);
    }

    protected abstract OAuthUserToken GetUserToken(
        ClaimsPrincipal principal,
        AuthenticationProperties properties,
        TOptions options);

    public Task ClearTokenAsync(
        AuthenticationScheme scheme,
        ClaimsPrincipal user,
        OAuthUserParameters? parameters = null,
        CancellationToken cancellationToken = default)
    {
        Ensure.Arg.NotNull(scheme);
        Ensure.Arg.NotNull(user);

        EnsureCompatibleScheme(scheme);

        return Task.CompletedTask;
    }
}