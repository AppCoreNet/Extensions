using System;
using AppCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace AppCore.DependencyInjection.Microsoft.Extensions
{
    public class MicrosoftContainer : IContainer
    {
        private readonly IServiceProvider _serviceProvider;

        public ContainerCapabilities Capabilities { get; } = ContainerCapabilities.None;

        public MicrosoftContainer(IServiceProvider serviceProvider)
        {
            Ensure.Arg.NotNull(serviceProvider, nameof(serviceProvider));
            _serviceProvider = serviceProvider;
        }

        public object Resolve(Type contractType)
        {
            return _serviceProvider.GetRequiredService(contractType);
        }

        public object ResolveOptional(Type contractType)
        {
            return _serviceProvider.GetService(contractType);
        }
    }
}