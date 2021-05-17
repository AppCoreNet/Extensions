// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using StructureMap.Pipeline;

namespace AppCore.DependencyInjection.StructureMap
{
    internal class ContainerLambdaInstance<T, TPluginType> : LambdaInstance<T, TPluginType>
        where T : TPluginType
    {
        public ContainerLambdaInstance(IComponentFactory<object> factory)
            : base(context => (T) factory.Create(context.GetInstance<IContainer>()))
        {
        }
    }
}