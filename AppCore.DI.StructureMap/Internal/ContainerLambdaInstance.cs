// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System;
using StructureMap.Pipeline;

namespace AppCore.DependencyInjection
{
    internal class ContainerLambdaInstance<T, TPluginType> : LambdaInstance<T, TPluginType>
        where T : TPluginType
    {
        public ContainerLambdaInstance(Func<IContainer, object> builder)
            : base(context => (T) builder(context.GetInstance<IContainer>()))
        {
        }
    }
}