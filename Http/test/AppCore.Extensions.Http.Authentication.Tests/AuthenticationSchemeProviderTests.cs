// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Options;
using NSubstitute;
using Xunit;

namespace AppCore.Extensions.Http.Authentication;

public class AuthenticationSchemeProviderTests
{
    private static AuthenticationSchemeProvider CreateAuthenticationSchemeProvider(HttpClientAuthenticationOptions? options = null)
    {
        options ??= new HttpClientAuthenticationOptions();
        var optionsMonitor = Substitute.For<IOptionsMonitor<HttpClientAuthenticationOptions>>();
        optionsMonitor.CurrentValue.Returns(options);
        var provider = new AuthenticationSchemeProvider(optionsMonitor);
        return provider;
    }

    [Fact]
    public async Task AddsSchemesFromOptions()
    {
        var scheme1 = new AuthenticationScheme("scheme1", typeof(IAuthenticationSchemeHandler<>), typeof(AuthenticationSchemeOptions));
        var scheme2 = new AuthenticationScheme("scheme2", typeof(IAuthenticationSchemeHandler<>), typeof(AuthenticationSchemeOptions));
        var options = new HttpClientAuthenticationOptions();
        options.AddScheme(scheme1);
        options.AddScheme(scheme2);

        using AuthenticationSchemeProvider provider = CreateAuthenticationSchemeProvider(options);

        IReadOnlyCollection<AuthenticationScheme> schemes = await provider.GetAllSchemesAsync();
        schemes.Should()
               .BeEquivalentTo(new[] { scheme1, scheme2 });
    }

    [Fact]
    public void AddSchemeThrowsOnDuplicateScheme()
    {
        using AuthenticationSchemeProvider provider = CreateAuthenticationSchemeProvider();

        var scheme = new AuthenticationScheme("scheme", typeof(IAuthenticationSchemeHandler<>), typeof(AuthenticationSchemeOptions));
        provider.AddScheme(scheme);

        Action func = () => provider.AddScheme(scheme);

        func.Should()
            .Throw<ArgumentException>();
    }

    [Fact]
    public async Task GetAllSchemesReturnsSchemes()
    {
        using AuthenticationSchemeProvider provider = CreateAuthenticationSchemeProvider();

        var scheme1 = new AuthenticationScheme("scheme1", typeof(IAuthenticationSchemeHandler<>), typeof(AuthenticationSchemeOptions));
        var scheme2 = new AuthenticationScheme("scheme2", typeof(IAuthenticationSchemeHandler<>), typeof(AuthenticationSchemeOptions));
        provider.AddScheme(scheme1);
        provider.AddScheme(scheme2);

        IReadOnlyCollection<AuthenticationScheme> schemes = await provider.GetAllSchemesAsync();
        schemes.Should()
               .BeEquivalentTo(new[] { scheme1, scheme2 });
    }

    [Fact]
    public async Task FindSchemeReturnsScheme()
    {
        using AuthenticationSchemeProvider provider = CreateAuthenticationSchemeProvider();

        var scheme1 = new AuthenticationScheme("scheme1", typeof(IAuthenticationSchemeHandler<>), typeof(AuthenticationSchemeOptions));
        var scheme2 = new AuthenticationScheme("scheme2", typeof(IAuthenticationSchemeHandler<>), typeof(AuthenticationSchemeOptions));
        provider.AddScheme(scheme1);
        provider.AddScheme(scheme2);

        AuthenticationScheme? result1 = await provider.FindSchemeAsync(scheme1.Name);
        result1.Should()
               .BeEquivalentTo(scheme1);

        AuthenticationScheme? result2 = await provider.FindSchemeAsync(scheme2.Name);
        result2.Should()
               .BeEquivalentTo(scheme2);
    }

    [Fact]
    public async Task FindUnknownSchemeReturnsNull()
    {
        using AuthenticationSchemeProvider provider = CreateAuthenticationSchemeProvider();
        AuthenticationScheme? result = await provider.FindSchemeAsync("scheme1");
        result.Should()
              .BeNull();
    }
}