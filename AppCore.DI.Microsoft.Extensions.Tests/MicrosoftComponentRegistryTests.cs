// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using Microsoft.Extensions.DependencyInjection;

namespace AppCore.DependencyInjection.Microsoft.Extensions
{
    public class MicrosoftComponentRegistryTests : ComponentRegistryTests
    {
        public IServiceCollection ServiceCollection { get; }

        public override IComponentRegistry Registry { get; }

        public MicrosoftComponentRegistryTests()
        {
            ServiceCollection = new ServiceCollection();
            Registry = new MicrosoftComponentRegistry(ServiceCollection);
        }

        protected override IContainer BuildContainer()
        {
            return new MicrosoftContainer(ServiceCollection.BuildServiceProvider());
        }
    }
}