using System;
using StructureMap.Pipeline;

namespace AppCore.DependencyInjection.StructureMap
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