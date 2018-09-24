// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using Microsoft.Extensions.DependencyInjection;

namespace AppCore.DependencyInjection
{
    public class MicrosoftComponentRegistryTests : ComponentRegistryTests
    {
        public IServiceCollection ServiceCollection { get; }

        public override IComponentRegistry Registry { get; }

        public MicrosoftComponentRegistryTests()
        {
            ServiceCollection = new ServiceCollection();
            Registry = new MicrosoftComponentRegistry();
        }

        protected override IContainer BuildContainer()
        {
            ((MicrosoftComponentRegistry)Registry).RegisterComponents(ServiceCollection);
            return new MicrosoftContainer(ServiceCollection.BuildServiceProvider());
        }
    }
}