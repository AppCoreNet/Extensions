// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using AppCore.Diagnostics;

namespace AppCore.DependencyInjection
{
    /// <summary>
    /// Creates instances of the <see cref="IComponentFactory{T}"/> interface.
    /// </summary>
    public class ComponentFactory
    {
        private class FactoryImpl<T> : IComponentFactory<T>
            where T : class
        {
            private readonly Func<IContainer, T> _factory;

            public FactoryImpl(Func<IContainer, T> factory)
            {
                _factory = factory;
            }

            public T Create(IContainer container)
            {
                return _factory(container);
            }
        }

        /// <summary>
        /// Creates an instance of the <see cref="IComponentFactory{T}"/> which instantiates objects
        /// using the provided <paramref name="factory"/> method.
        /// </summary>
        /// <typeparam name="T">The type of the object which is being instantiated.</typeparam>
        /// <param name="factory">The factory method.</param>
        /// <returns>An instance of the <see cref="IComponentFactory{T}"/> interface.</returns>
        public static IComponentFactory<T> Create<T>(Func<IContainer, T> factory)
            where T : class
        {
            Ensure.Arg.NotNull(factory, nameof(factory));
            return new FactoryImpl<T>(factory);
        }
    }
}