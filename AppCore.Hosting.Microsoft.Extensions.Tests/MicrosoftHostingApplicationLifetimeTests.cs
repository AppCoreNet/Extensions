// Licensed under the MIT License.
// Copyright (c) 2020 the AppCore .NET project.

using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit;

namespace AppCore.Hosting.Microsoft.Extensions
{
    public class MicrosoftHostingApplicationLifetimeTests
    {
        [Fact]
        public async Task StartIsSignaled()
        {
            var builder = new HostBuilder();
            builder.ConfigureAppCore();
            IHost host = builder.Build();

            var lifetime = host.Services.GetService<IApplicationLifetime>();

            lifetime.Started.IsCompleted.Should()
                    .BeFalse();

            await host.StartAsync();
            try
            {
                await lifetime.Started;
            }
            finally
            {
                await host.StopAsync();
            }
        }

        [Fact]
        public async Task StopIsSignaled()
        {
            var builder = new HostBuilder();
            builder.ConfigureAppCore();
            IHost host = builder.Build();

            var lifetime = host.Services.GetService<IApplicationLifetime>();

            await host.StartAsync();
            try
            {
                lifetime.Stopping.IsCompleted.Should()
                        .BeFalse();

                lifetime.Stopped.IsCompleted.Should()
                        .BeFalse();

                Task stop = host.StopAsync();

                await lifetime.Stopping;
                await stop;

                await lifetime.Stopped;
            }
            catch
            {
                await host.StopAsync();
                throw;
            }
        }

        [Fact]
        public async Task StopWillShutdown()
        {
            var builder = new HostBuilder();
            builder.ConfigureAppCore();
            IHost host = builder.Build();

            var lifetime = host.Services.GetService<IApplicationLifetime>();

            await host.StartAsync();
            try
            {
                lifetime.Stop();
                await host.StopAsync();
            }
            catch
            {
                await host.StopAsync();
                throw;
            }
        }

    }
}