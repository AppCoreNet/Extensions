// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using Microsoft.Extensions.DependencyInjection;

namespace AppCore.DependencyInjection.Microsoft.Extensions
{
    public class MicrosoftContainerScopeTests : ContainerScopeTests
    {
        public IServiceCollection ServiceCollection { get; }

        public override IComponentRegistry Registry { get; }

        public MicrosoftContainerScopeTests()
        {
            ServiceCollection = new ServiceCollection();
            Registry = new MicrosoftComponentRegistry();
        }

        public override IContainerScope CreateScope()
        {
            ((MicrosoftComponentRegistry)Registry).RegisterComponents(ServiceCollection);

            return new MicrosoftContainerScope(
                ServiceCollection.BuildServiceProvider()
                                 .GetRequiredService<IServiceScopeFactory>()
                                 .CreateScope());
        }
    }
}
