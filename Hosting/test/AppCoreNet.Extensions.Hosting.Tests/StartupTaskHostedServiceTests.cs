// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace AppCore.Extensions.Hosting;

public class StartupTaskHostedServiceTests
{
    [Fact]
    public async Task ExecutesAll()
    {
        var task1 = Substitute.For<IStartupTask>();
        var task2 = Substitute.For<IStartupTask>();
        var logger = Substitute.For<ILogger<StartupTaskHostedService>>();

        var executor = new StartupTaskHostedService(new[] { task1, task2 }, logger);
        await executor.StartAsync(CancellationToken.None);

        await task1.Received(1)
                   .ExecuteAsync(Arg.Any<CancellationToken>());

        await task2.Received(1)
                   .ExecuteAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task ExecuteThrowsForAny()
    {
        var task1 = Substitute.For<IStartupTask>();

        var task2 = Substitute.For<IStartupTask>();
        task2.ExecuteAsync(Arg.Any<CancellationToken>())
             .ThrowsAsync(new InvalidOperationException());

        var logger = Substitute.For<ILogger<StartupTaskHostedService>>();

        var executor = new StartupTaskHostedService(new[] { task1, task2 }, logger);
        await Assert.ThrowsAsync<InvalidOperationException>(
            async () => await executor.StartAsync(CancellationToken.None));
    }
}