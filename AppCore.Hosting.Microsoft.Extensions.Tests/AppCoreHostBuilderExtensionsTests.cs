// Licensed under the MIT License.
// Copyright (c) 2018-2020 the AppCore .NET project.

using AppCore.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit;

namespace AppCore.Hosting.Microsoft.Extensions
{
    public class AppCoreHostBuilderExtensionsTests
    {
        [Fact]
        public void ConfigureAppCore()
        {
            Host.CreateDefaultBuilder()
                .ConfigureAppCore(
                    r =>
                    {
                        r.AddPlugins();
                        r.AddFacilitiesFrom(s => s.Plugins());
                        r.TryAddFrom(
                            r => r
                                .Plugins(
                                    p => p
                                        .WithContract<string>()));
                    })
                .Build();
        }
    }
}
