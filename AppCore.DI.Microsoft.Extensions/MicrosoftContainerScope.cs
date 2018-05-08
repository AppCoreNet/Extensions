using System;
using AppCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace AppCore.DependencyInjection.Microsoft.Extensions
{
    public class MicrosoftContainerScope : IContainerScope
    {
        private readonly IServiceScope _serviceScope;
        private readonly MicrosoftContainer _container;

        public IContainer Container => _container;

        public MicrosoftContainerScope(IServiceScope serviceScope)
        {
            Ensure.Arg.NotNull(serviceScope, nameof(serviceScope));

            _serviceScope = serviceScope;
            _container = new MicrosoftContainer(serviceScope.ServiceProvider);
        }

        ~MicrosoftContainerScope()
        {
            Dispose(false);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                _serviceScope.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}