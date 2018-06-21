// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using AppCore.Diagnostics;

namespace AppCore.DependencyInjection.Builder
{
    /// <summary>
    /// Provides extension methods for the <see cref="IRegistrationBuilder"/> and <see cref="IRegistrationBuilder{TContract}"/>
    /// interfaces.
    /// </summary>
    public static class RegistrationBuilderExtensions
    {
        /// <summary>
        /// Specifies the default lifetime for components.
        /// </summary>
        /// <param name="builder">The <see cref="IRegistrationBuilder"/>.</param>
        /// <param name="lifetime">The default <see cref="ComponentLifetime"/>.</param>
        /// <returns>The <see cref="IRegistrationBuilder"/></returns>
        public static IRegistrationBuilder WithDefaultLifetime(
            this IRegistrationBuilder builder,
            ComponentLifetime lifetime)
        {
            Ensure.Arg.NotNull(builder, nameof(builder));

            builder.DefaultLifetime = lifetime;
            return builder;
        }

        /// <summary>
        /// Specifies the default lifetime for components.
        /// </summary>
        /// <typeparam name="TContract">The type of the contract.</typeparam>
        /// <param name="builder">The <see cref="IRegistrationBuilder{TContract}"/>.</param>
        /// <param name="lifetime">The default <see cref="ComponentLifetime"/>.</param>
        /// <returns>The <see cref="IRegistrationBuilder{TContract}"/>.</returns>
        public static IRegistrationBuilder<TContract> WithDefaultLifetime<TContract>(
            this IRegistrationBuilder<TContract> builder,
            ComponentLifetime lifetime)
        {
            Ensure.Arg.NotNull(builder, nameof(builder));

            builder.DefaultLifetime = lifetime;
            return builder;
        }
    }
}
