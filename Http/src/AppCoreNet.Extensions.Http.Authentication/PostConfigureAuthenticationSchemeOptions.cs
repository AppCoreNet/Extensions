using System;
using Microsoft.Extensions.Options;

namespace AppCoreNet.Extensions.Http.Authentication;

internal sealed class PostConfigureAuthenticationSchemeOptions<TOptions> : IPostConfigureOptions<TOptions>
    where TOptions : AuthenticationSchemeOptions
{
    private readonly TimeProvider _timeProvider;

    public PostConfigureAuthenticationSchemeOptions(TimeProvider timeProvider)
    {
        _timeProvider = timeProvider;
    }

    public void PostConfigure(string? name, TOptions options)
    {
        options.TimeProvider ??= _timeProvider;
    }
}