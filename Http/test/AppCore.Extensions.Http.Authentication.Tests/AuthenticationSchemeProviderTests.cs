// Licensed under the MIT License.
// Copyright (c) 2018-2022 the AppCore .NET project.

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
    [Fact]
    public async Task AddsSchemesFromOptions()
    {
        var scheme1 = new AuthenticationScheme("scheme1", typeof(IAuthenticationSchemeHandler<>));
        var scheme2 = new AuthenticationScheme("scheme2", typeof(IAuthenticationSchemeHandler<>));

        var optionsMonitor = Substitute.For<IOptionsMonitor<HttpClientAuthenticationOptions>>();
        var options = new HttpClientAuthenticationOptions();
        options.AddScheme(scheme1);
        options.AddScheme(scheme2);
        optionsMonitor.CurrentValue.Returns(options);

        var provider = new AuthenticationSchemeProvider(optionsMonitor);

        IReadOnlyCollection<AuthenticationScheme> schemes = await provider.GetAllSchemesAsync();
        schemes.Should()
               .BeEquivalentTo(new[] { scheme1, scheme2 });
    }

    [Fact]
    public void AddSchemeThrowsOnDuplicateScheme()
    {
        var options = Substitute.For<IOptionsMonitor<HttpClientAuthenticationOptions>>();
        options.CurrentValue.Returns(new HttpClientAuthenticationOptions());

        var provider = new AuthenticationSchemeProvider(options);

        var scheme = new AuthenticationScheme("scheme", typeof(IAuthenticationSchemeHandler<>));
        provider.AddScheme(scheme);

        Action func = () => provider.AddScheme(scheme);

        func.Should()
            .Throw<ArgumentException>();
    }
}