// Licensed under the MIT License.
// Copyright (c) 2018-2020 the AppCore .NET project.

using System;
using AppCore.DependencyInjection;
using AppCore.DependencyInjection.Activator;
using Microsoft.Extensions.Configuration;

namespace AppCore.Hosting.Microsoft.Extensions
{
#if NET461 || NETCOREAPP3_0_OR_GREATER
    using IHostEnvironment = global::Microsoft.Extensions.Hosting.IHostEnvironment;
#else
    using IHostEnvironment = global::Microsoft.Extensions.Hosting.IHostingEnvironment;
#endif

    internal class StartupContainer : IContainer
    {
        private readonly IHostEnvironment _environment;
        private readonly IConfiguration _configuration;
        private readonly ContainerActivator _activator;

        public ContainerCapabilities Capabilities { get; } = ContainerCapabilities.None;
        
        public StartupContainer(IHostEnvironment environment, IConfiguration configuration)
        {
            _environment = environment;
            _configuration = configuration;
            _activator = new ContainerActivator(this);
        }

        public object Resolve(Type contractType)
        {
            object result = ResolveOptional(contractType);
            if (result == null)
            {
                throw new InvalidOperationException($"Could not resolve a service with type '{contractType}'.");
            }

            return result;
        }

        public object ResolveOptional(Type contractType)
        {
            if (contractType == typeof(IContainer))
                return this;

            if (contractType == typeof(IActivator))
                return _activator;

            if (contractType == typeof(IHostEnvironment))
                return _environment;

            if (contractType == typeof(IConfiguration))
                return _configuration;

            return null;
        }
   }
}