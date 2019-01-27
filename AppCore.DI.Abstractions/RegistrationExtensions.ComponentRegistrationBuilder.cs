// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System;
using AppCore.DependencyInjection.Builder;
using AppCore.Diagnostics;

namespace AppCore.DependencyInjection
{
    /// <summary>
    /// Provides extension methods for registering components with the <see cref="IComponentRegistry"/>.
    /// </summary>
    // ReSharper disable PossibleStructMemberModificationOfNonVariableStruct
    public static partial class RegistrationExtensions
    {
        /// <summary>
        /// Specifies the lifetime of the registered component.
        /// </summary>
        /// <typeparam name="TRegistrationInfo">The registration information.</typeparam>
        /// <param name="builder">The <see cref="IComponentRegistrationBuilder{TRegistrationInfo}"/>.</param>
        /// <param name="lifetime">The lifetime of the coponent.</param>
        /// <returns>The <paramref name="builder"/>.</returns>
        /// <exception cref="ArgumentNullException">Argument <paramref name="builder"/> is <c>null</c>.</exception>
        public static IComponentRegistrationBuilder<TRegistrationInfo> WithLifetime<TRegistrationInfo>(
            this IComponentRegistrationBuilder<TRegistrationInfo> builder,
            ComponentLifetime lifetime)
        where TRegistrationInfo : IComponentRegistrationInfoWithLifetime
        {
            Ensure.Arg.NotNull(builder, nameof(builder));
            builder.RegistrationInfo.Lifetime = lifetime;
            return builder;
        }

        /// <summary>
        /// Specifies the lifetime of the registered component.
        /// </summary>
        /// <typeparam name="TContract">The contract implemented by the component.</typeparam>
        /// <typeparam name="TRegistrationInfo">The registration information.</typeparam>
        /// <param name="builder">The <see cref="IComponentRegistrationBuilder{TContract, TRegistrationInfo}"/>.</param>
        /// <param name="lifetime">The lifetime of the coponent.</param>
        /// <returns>The <paramref name="builder"/>.</returns>
        /// <exception cref="ArgumentNullException">Argument <paramref name="builder"/> is <c>null</c>.</exception>
        public static IComponentRegistrationBuilder<TContract, TRegistrationInfo> WithLifetime<TContract, TRegistrationInfo>(
            this IComponentRegistrationBuilder<TContract, TRegistrationInfo> builder,
            ComponentLifetime lifetime)
            where TRegistrationInfo : IComponentRegistrationInfoWithLifetime
        {
            Ensure.Arg.NotNull(builder, nameof(builder));
            builder.RegistrationInfo.Lifetime = lifetime;
            return builder;
        }

        /// <summary>
        /// Specifies that the component must be instantiated per dependency.
        /// </summary>
        /// <typeparam name="TRegistrationInfo">The registration information.</typeparam>
        /// <param name="builder">The <see cref="IComponentRegistrationBuilder{TRegistrationInfo}"/>.</param>
        /// <returns>The <paramref name="builder"/>.</returns>
        /// <exception cref="ArgumentNullException">Argument <paramref name="builder"/> is <c>null</c>.</exception>
        public static IComponentRegistrationBuilder<TRegistrationInfo> PerDependency<TRegistrationInfo>(
            this IComponentRegistrationBuilder<TRegistrationInfo> builder)
            where TRegistrationInfo : IComponentRegistrationInfoWithLifetime
        {
            return builder.WithLifetime(ComponentLifetime.Transient);
        }

        /// <summary>
        /// Specifies that the component must be instantiated per dependency.
        /// </summary>
        /// <typeparam name="TContract">The contract implemented by the component.</typeparam>
        /// <typeparam name="TRegistrationInfo">The registration information.</typeparam>
        /// <param name="builder">The <see cref="IComponentRegistrationBuilder{TContract, TRegistrationInfo}"/>.</param>
        /// <returns>The <paramref name="builder"/>.</returns>
        /// <exception cref="ArgumentNullException">Argument <paramref name="builder"/> is <c>null</c>.</exception>
        public static IComponentRegistrationBuilder<TContract, TRegistrationInfo> PerDependency<TContract, TRegistrationInfo>(
            this IComponentRegistrationBuilder<TContract, TRegistrationInfo> builder)
            where TRegistrationInfo : IComponentRegistrationInfoWithLifetime
        {
            return builder.WithLifetime(ComponentLifetime.Transient);
        }

        /// <summary>
        /// Specifies that the component must be instantiated per container scope.
        /// </summary>
        /// <typeparam name="TRegistrationInfo">The registration information.</typeparam>
        /// <param name="builder">The <see cref="IComponentRegistrationBuilder{TRegistrationInfo}"/>.</param>
        /// <returns>The <paramref name="builder"/>.</returns>
        /// <exception cref="ArgumentNullException">Argument <paramref name="builder"/> is <c>null</c>.</exception>
        public static IComponentRegistrationBuilder<TRegistrationInfo> PerScope<TRegistrationInfo>(
            this IComponentRegistrationBuilder<TRegistrationInfo> builder)
            where TRegistrationInfo : IComponentRegistrationInfoWithLifetime
        {
            return builder.WithLifetime(ComponentLifetime.Scoped);
        }

        /// <summary>
        /// Specifies that the component must be instantiated per container scope.
        /// </summary>
        /// <typeparam name="TRegistrationInfo">The registration information.</typeparam>
        /// <typeparam name="TContract">The contract implemented by the component.</typeparam>
        /// <param name="builder">The <see cref="IComponentRegistrationBuilder{TContract, TRegistrationInfo}"/>.</param>
        /// <returns>The <paramref name="builder"/>.</returns>
        /// <exception cref="ArgumentNullException">Argument <paramref name="builder"/> is <c>null</c>.</exception>
        public static IComponentRegistrationBuilder<TContract, TRegistrationInfo> PerScope<TContract, TRegistrationInfo>(
            this IComponentRegistrationBuilder<TContract, TRegistrationInfo> builder)
            where TRegistrationInfo : IComponentRegistrationInfoWithLifetime
        {
            return builder.WithLifetime(ComponentLifetime.Scoped);
        }

        /// <summary>
        /// Specifies that the component must be instantiated only once.
        /// </summary>
        /// <typeparam name="TRegistrationInfo">The registration information.</typeparam>
        /// <param name="builder">The <see cref="IComponentRegistrationBuilder{TRegistrationInfo}"/>.</param>
        /// <returns>The <paramref name="builder"/>.</returns>
        /// <exception cref="ArgumentNullException">Argument <paramref name="builder"/> is <c>null</c>.</exception>
        public static IComponentRegistrationBuilder<TRegistrationInfo> PerContainer<TRegistrationInfo>(
            this IComponentRegistrationBuilder<TRegistrationInfo> builder)
            where TRegistrationInfo : IComponentRegistrationInfoWithLifetime
        {
            return builder.WithLifetime(ComponentLifetime.Singleton);
        }

        /// <summary>
        /// Specifies that the component must be instantiated only once.
        /// </summary>
        /// <typeparam name="TContract">The contract implemented by the component.</typeparam>
        /// <typeparam name="TRegistrationInfo">The registration information.</typeparam>
        /// <param name="builder">The <see cref="IComponentRegistrationBuilder{TContract, TRegistrationInfo}"/>.</param>
        /// <returns>The <paramref name="builder"/>.</returns>
        /// <exception cref="ArgumentNullException">Argument <paramref name="builder"/> is <c>null</c>.</exception>
        public static IComponentRegistrationBuilder<TContract, TRegistrationInfo> PerContainer<TContract, TRegistrationInfo>(
            this IComponentRegistrationBuilder<TContract, TRegistrationInfo> builder)
            where TRegistrationInfo : IComponentRegistrationInfoWithLifetime
        {
            return builder.WithLifetime(ComponentLifetime.Singleton);
        }

        /// <summary>
        /// Skips registration if component with same contract and implementation is already registered.
        /// </summary>
        /// <typeparam name="TRegistrationInfo">The registration information.</typeparam>
        /// <param name="builder">The <see cref="IComponentRegistrationBuilder{TRegistrationInfo}"/>.</param>
        /// <returns>The <paramref name="builder"/>.</returns>
        /// <exception cref="ArgumentNullException">Argument <paramref name="builder"/> is <c>null</c>.</exception>
        public static IComponentRegistrationBuilder<TRegistrationInfo> IfNotRegistered<TRegistrationInfo>(
            this IComponentRegistrationBuilder<TRegistrationInfo> builder)
            where TRegistrationInfo : IComponentRegistrationInfo
        {
            Ensure.Arg.NotNull(builder, nameof(builder));
            builder.RegistrationInfo.Flags |= ComponentRegistrationFlags.IfNotRegistered;
            return builder;
        }

        /// <summary>
        /// Skips registration if component with same contract and implementation is already registered.
        /// </summary>
        /// <typeparam name="TContract">The contract implemented by the component.</typeparam>
        /// <typeparam name="TRegistrationInfo">The registration information.</typeparam>
        /// <param name="builder">The <see cref="IComponentRegistrationBuilder{TContract, TRegistrationInfo}"/>.</param>
        /// <returns>The <paramref name="builder"/>.</returns>
        /// <exception cref="ArgumentNullException">Argument <paramref name="builder"/> is <c>null</c>.</exception>
        public static IComponentRegistrationBuilder<TContract, TRegistrationInfo> IfNotRegistered<
            TContract, TRegistrationInfo>(
            this IComponentRegistrationBuilder<TContract, TRegistrationInfo> builder)
            where TRegistrationInfo : IComponentRegistrationInfo
        {
            Ensure.Arg.NotNull(builder, nameof(builder));
            builder.RegistrationInfo.Flags |= ComponentRegistrationFlags.IfNotRegistered;
            return builder;
        }

        /// <summary>
        /// Skips registration if a component with same contract is already registered.
        /// </summary>
        /// <typeparam name="TRegistrationInfo">The registration information.</typeparam>
        /// <param name="builder">The <see cref="IComponentRegistrationBuilder{TRegistrationInfo}"/>.</param>
        /// <returns>The <paramref name="builder"/>.</returns>
        /// <exception cref="ArgumentNullException">Argument <paramref name="builder"/> is <c>null</c>.</exception>
        public static IComponentRegistrationBuilder<TRegistrationInfo> IfNoneRegistered<TRegistrationInfo>(
            this IComponentRegistrationBuilder<TRegistrationInfo> builder)
            where TRegistrationInfo : IComponentRegistrationInfo
        {
            Ensure.Arg.NotNull(builder, nameof(builder));
            builder.RegistrationInfo.Flags |= ComponentRegistrationFlags.IfNoneRegistered;
            return builder;
        }

        /// <summary>
        /// Skips registration if a component with same contract is already registered.
        /// </summary>
        /// <typeparam name="TContract">The contract implemented by the component.</typeparam>
        /// <typeparam name="TRegistrationInfo">The registration information.</typeparam>
        /// <param name="builder">The <see cref="IComponentRegistrationBuilder{TContract, TRegistrationInfo}"/>.</param>
        /// <returns>The <paramref name="builder"/>.</returns>
        /// <exception cref="ArgumentNullException">Argument <paramref name="builder"/> is <c>null</c>.</exception>
        public static IComponentRegistrationBuilder<TContract, TRegistrationInfo> IfNoneRegistered<
            TContract, TRegistrationInfo>(
            this IComponentRegistrationBuilder<TContract, TRegistrationInfo> builder)
            where TRegistrationInfo : IComponentRegistrationInfo
        {
            Ensure.Arg.NotNull(builder, nameof(builder));
            builder.RegistrationInfo.Flags |= ComponentRegistrationFlags.IfNoneRegistered;
            return builder;
        }
    }
}
