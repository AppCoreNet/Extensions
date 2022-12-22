using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Options;

namespace AppCore.Extensions.Http.Authentication.OAuth.AspNetCore.OpenIdConnect;

internal sealed class OpenIdConnectUserOptionsResolver
    : AuthenticationSchemeOAuthClientOptionsResolver<
        OpenIdConnectUserOptions,
        OpenIdConnectOptions,
        OpenIdConnectHandler
    >
{
    public OpenIdConnectUserOptionsResolver(
        Microsoft.AspNetCore.Authentication.IAuthenticationSchemeProvider authenticationSchemeProvider,
        IOptionsMonitor<OpenIdConnectUserOptions> clientOptions,
        IOptionsMonitor<OpenIdConnectOptions> authenticationSchemeOptions)
        : base(authenticationSchemeProvider, authenticationSchemeOptions, clientOptions)
    {
    }

    protected override string? GetSchemeName(OpenIdConnectUserOptions options)
    {
        return options.ChallengeScheme;
    }

    protected override async Task<OAuthClientOptions> GetOptionsFromSchemeAsync(
        OpenIdConnectUserOptions clientOptions,
        OpenIdConnectOptions oidcOptions)
    {
        OAuthClientOptions result = await oidcOptions.GetOAuthClientOptionsAsync(default);
        return result;
    }
}