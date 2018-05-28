// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using AppCore.Diagnostics;

namespace AppCore.DependencyInjection.Builder
{
    public static class ComponentRegistrationBuilderExtensions
    {
        public static IComponentRegistrationBuilder<TRegistrationInfo> WithLifetime<TRegistrationInfo>(
            this IComponentRegistrationBuilder<TRegistrationInfo> builder,
            ComponentLifetime lifetime)
        where TRegistrationInfo : IComponentRegistrationInfoWithLifetime
        {
            Ensure.Arg.NotNull(builder, nameof(builder));
            builder.RegistrationInfo.Lifetime = lifetime;
            return builder;
        }

        public static IComponentRegistrationBuilder<TContract, TRegistrationInfo> WithLifetime<TContract, TRegistrationInfo>(
            this IComponentRegistrationBuilder<TContract, TRegistrationInfo> builder,
            ComponentLifetime lifetime)
            where TRegistrationInfo : IComponentRegistrationInfoWithLifetime
        {
            Ensure.Arg.NotNull(builder, nameof(builder));
            builder.RegistrationInfo.Lifetime = lifetime;
            return builder;
        }

        public static IComponentRegistrationBuilder<TRegistrationInfo> PerDependency<TRegistrationInfo>(
            this IComponentRegistrationBuilder<TRegistrationInfo> builder)
            where TRegistrationInfo : IComponentRegistrationInfoWithLifetime
        {
            builder.WithLifetime(ComponentLifetime.Transient);
            return builder;
        }

        public static IComponentRegistrationBuilder<TContract, TRegistrationInfo> PerDependency<TContract, TRegistrationInfo>(
            this IComponentRegistrationBuilder<TContract, TRegistrationInfo> builder)
            where TRegistrationInfo : IComponentRegistrationInfoWithLifetime
        {
            builder.WithLifetime(ComponentLifetime.Transient);
            return builder;
        }

        public static IComponentRegistrationBuilder<TRegistrationInfo> PerScope<TRegistrationInfo>(
            this IComponentRegistrationBuilder<TRegistrationInfo> builder)
            where TRegistrationInfo : IComponentRegistrationInfoWithLifetime
        {
            builder.WithLifetime(ComponentLifetime.Scoped);
            return builder;
        }

        public static IComponentRegistrationBuilder<TContract, TRegistrationInfo> PerScope<TContract, TRegistrationInfo>(
            this IComponentRegistrationBuilder<TContract, TRegistrationInfo> builder)
            where TRegistrationInfo : IComponentRegistrationInfoWithLifetime
        {
            builder.WithLifetime(ComponentLifetime.Scoped);
            return builder;
        }

        public static IComponentRegistrationBuilder<TRegistrationInfo> PerContainer<TRegistrationInfo>(
            this IComponentRegistrationBuilder<TRegistrationInfo> builder)
            where TRegistrationInfo : IComponentRegistrationInfoWithLifetime
        {
            builder.WithLifetime(ComponentLifetime.Singleton);
            return builder;
        }

        public static IComponentRegistrationBuilder<TContract, TRegistrationInfo> PerContainer<TContract, TRegistrationInfo>(
            this IComponentRegistrationBuilder<TContract, TRegistrationInfo> builder)
            where TRegistrationInfo : IComponentRegistrationInfoWithLifetime
        {
            builder.WithLifetime(ComponentLifetime.Singleton);
            return builder;
        }

        /// <summary>
        /// Skips registration if component with same contract and implementation is already registered.
        /// </summary>
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