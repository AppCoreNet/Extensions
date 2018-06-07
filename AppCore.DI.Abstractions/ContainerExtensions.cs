// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System;
using System.Collections.Generic;
using AppCore.Diagnostics;

namespace AppCore.DependencyInjection
{
    /// <summary>
    /// Provides extension methods for the <see cref="IContainer"/> interface.
    /// </summary>
    /// <seealso cref="IContainer"/>
    public static class ContainerExtensions
    {
        /// <summary>
        /// Resolves the component which implements <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The contract of the component to resolve.</typeparam>
        /// <param name="container">The <see cref="IContainer"/>.</param>
        /// <returns>The resolved component.</returns>
        /// <exception cref="ArgumentNullException">Argument <paramref name="container"/> is <c>null</c>.</exception>
        public static T Resolve<T>(this IContainer container)
        {
            Ensure.Arg.NotNull(container, nameof(container));
            return (T) container.Resolve(typeof(T));
        }

        /// <summary>
        /// Tries to resolve the component which implements <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The contract of the component to resolve.</typeparam>
        /// <param name="container">The <see cref="IContainer"/>.</param>
        /// <returns>The resolved component; <c>null</c> if no component for <typeparamref name="T"/> was registered.</returns>
        /// <exception cref="ArgumentNullException">Argument <paramref name="container"/> is <c>null</c>.</exception>
        public static T ResolveOptional<T>(this IContainer container)
        {
            Ensure.Arg.NotNull(container, nameof(container));
            return (T) container.ResolveOptional(typeof(T));
        }

        /// <summary>
        /// Resolves all components which implement <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The contract of the components to resolve.</typeparam>
        /// <param name="container">The <see cref="IContainer"/>.</param>
        /// <returns>The resolved components.</returns>
        /// <exception cref="ArgumentNullException">Argument <paramref name="container"/> is <c>null</c>.</exception>
        public static IEnumerable<T> ResolveAll<T>(this IContainer container)
        {
            Ensure.Arg.NotNull(container, nameof(container));
            return (IEnumerable<T>)container.Resolve(typeof(IEnumerable<T>));
        }

        /// <summary>
        /// Creates a scoped <see cref="IContainer"/>.
        /// </summary>
        /// <remarks>
        /// The <see cref="IContainerScope"/> must be disposed.
        /// </remarks>
        /// <param name="container">The <see cref="IContainer"/>.</param>
        /// <returns>The <see cref="IContainerScope"/>.</returns>
        /// <exception cref="ArgumentNullException">Argument <paramref name="container"/> is <c>null</c>.</exception>
        public static IContainerScope CreateScope(this IContainer container)
        {
            Ensure.Arg.NotNull(container, nameof(container));
            return container.Resolve<IContainerScopeFactory>()
                            .CreateScope();
        }
    }
}