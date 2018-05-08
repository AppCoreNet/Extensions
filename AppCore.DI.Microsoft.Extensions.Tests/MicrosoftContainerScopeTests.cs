using System;
using System.Collections.Generic;
using System.Text;
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
            Registry = new MicrosoftComponentRegistry(ServiceCollection);
        }

        public override IContainerScope CreateScope()
        {
            return new MicrosoftContainerScope(
                ServiceCollection.BuildServiceProvider()
                                 .GetRequiredService<IServiceScopeFactory>()
                                 .CreateScope());
        }
    }
}
