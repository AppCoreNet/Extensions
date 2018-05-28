// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System.Collections.Generic;
using AppCore.Diagnostics;

namespace AppCore.DependencyInjection
{
    public static class ContainerExtensions
    {
        public static T Resolve<T>(this IContainer container)
        {
            Ensure.Arg.NotNull(container, nameof(container));
            return (T) container.Resolve(typeof(T));
        }

        public static T ResolveOptional<T>(this IContainer container)
        {
            Ensure.Arg.NotNull(container, nameof(container));
            return (T) container.ResolveOptional(typeof(T));
        }

        public static IEnumerable<T> ResolveAll<T>(this IContainer container)
        {
            Ensure.Arg.NotNull(container, nameof(container));
            return (IEnumerable<T>)container.Resolve(typeof(IEnumerable<T>));
        }

        public static IContainerScope CreateScope(this IContainer container)
        {
            Ensure.Arg.NotNull(container, nameof(container));
            return container.Resolve<IContainerScopeFactory>()
                            .CreateScope();
        }
    }
}