// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System;
using Microsoft.Extensions.DependencyInjection;

namespace AppCore.DependencyInjection
{
    internal class ImplementationFactoryWrapper
    {
        private readonly Func<IContainer, object> _target;

        public Type LimitType { get; }

        public ImplementationFactoryWrapper(Func<IContainer, object> target, Type limitType)
        {
            _target = target;
            LimitType = limitType;
        }

        public object CreateInstance(IServiceProvider provider)
        {
            return _target(provider.GetRequiredService<IContainer>());
        }
    }
}