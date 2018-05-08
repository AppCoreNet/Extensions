using AppCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace AppCore.DependencyInjection.Microsoft.Extensions
{
    public class MicrosoftContainerScopeFactory : IContainerScopeFactory
    {
        private readonly IServiceScopeFactory _factory;

        public MicrosoftContainerScopeFactory(IServiceScopeFactory factory)
        {
            Ensure.Arg.NotNull(factory, nameof(factory));
            _factory = factory;
        }

        public IContainerScope CreateScope()
        {
            return new MicrosoftContainerScope(_factory.CreateScope());
        }
    }
}