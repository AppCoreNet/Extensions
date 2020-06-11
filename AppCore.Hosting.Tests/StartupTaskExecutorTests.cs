// Licensed under the MIT License.
// Copyright (c) 2020 the AppCore .NET project.

using System;
using System.Threading;
using System.Threading.Tasks;
using AppCore.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace AppCore.Hosting
{
    public class StartupTaskExecutorTests
    {
        [Fact]
        public async Task ExecutesAll()
        {
            var task1 = Substitute.For<IStartupTask>();
            var task2 = Substitute.For<IStartupTask>();
            var logger = Substitute.For<ILogger<StartupTaskExecutor>>();

            var executor = new StartupTaskExecutor(new []{task1,task2}, logger);
            await executor.ExecuteAsync(CancellationToken.None);

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
                 .Throws(new InvalidOperationException());

            var logger = Substitute.For<ILogger<StartupTaskExecutor>>();

            var executor = new StartupTaskExecutor(new[] {task1, task2}, logger);
            await Assert.ThrowsAsync<InvalidOperationException>(
                async () => await executor.ExecuteAsync(CancellationToken.None));
        }
    }
}