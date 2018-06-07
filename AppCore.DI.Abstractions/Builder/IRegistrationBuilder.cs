// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System;
using System.ComponentModel;

namespace AppCore.DependencyInjection.Builder
{
    /// <summary>
    /// Represents a type to register components.
    /// </summary>
    /// <seealso cref="IComponentRegistry"/>
    public interface IRegistrationBuilder
    {
        /// <summary>
        /// Gets the <see cref="IComponentRegistry"/>.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        IComponentRegistry Registry { get; }

        /// <summary>
        /// Gets the contract of the components being registered.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        Type ContractType { get; }

        /// <summary>
        /// Adds <typeparamref name="TComponent"/> using constructor to the registration.
        /// </summary>
        /// <typeparam name="TComponent">The type of the component to register.</typeparam>
        /// <returns>
        ///     A <see cref="IComponentRegistrationBuilder{TRegistrationInfo}"/> used to configure the
        ///     component registration.
        /// </returns>
        /// <exception cref="ArgumentException">The <typeparamref name="TComponent"/> does not implement the contract.</exception>
        IComponentRegistrationBuilder<TypeRegistrationInfo> Add<TComponent>();

        /// <summary>
        /// Adds <typeparamref name="TComponent"/> using factory to the registration.
        /// </summary>
        /// <typeparam name="TComponent">The type of the component to register.</typeparam>
        /// <param name="factory">The delegate used to instantiate the component.</param>
        /// <returns>
        ///     A <see cref="IComponentRegistrationBuilder{TRegistrationInfo}"/> used to configure the
        ///     component registration.
        /// </returns>
        /// <exception cref="ArgumentNullException">The argument <paramref name="factory"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">The <typeparamref name="TComponent"/> does not implement the contract.</exception>
        IComponentRegistrationBuilder<DelegateRegistrationInfo> Add<TComponent>(
            Func<IContainer, TComponent> factory);

        /// <summary>
        /// Adds <typeparamref name="TComponent"/> using instance to the registration.
        /// </summary>
        /// <typeparam name="TComponent">The type of the component to register.</typeparam>
        /// <param name="instance">The instance of the component.</param>
        /// <returns>
        ///     A <see cref="IComponentRegistrationBuilder{TRegistrationInfo}"/> used to configure the
        ///     component registration.
        /// </returns>
        /// <exception cref="ArgumentNullException">The argument <paramref name="instance"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">The <typeparamref name="TComponent"/> does not implement the contract.</exception>
        IComponentRegistrationBuilder<SingleInstanceRegistrationInfo> Add<TComponent>(
            TComponent instance);

        /// <summary>
        /// Adds <paramref name="componentType"/> using constructor to the registration.
        /// </summary>
        /// <returns>
        ///     A <see cref="IComponentRegistrationBuilder{TRegistrationInfo}"/> used to configure the
        ///     component registration.
        /// </returns>
        /// <exception cref="ArgumentNullException">The argument <paramref name="componentType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">The <paramref name="componentType"/> does not implement the contract.</exception>
        IComponentRegistrationBuilder<TypeRegistrationInfo> Add(Type componentType);
    }

    /// <summary>
    /// Represents a type to register components.
    /// </summary>
    /// <typeparam name="TContract">The contract of components being registered.</typeparam>
    /// <seealso cref="IComponentRegistry"/>
    public interface IRegistrationBuilder<TContract>
    {
        /// <summary>
        /// Gets the <see cref="IComponentRegistry"/>.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        IComponentRegistry Registry { get; }

        /// <summary>
        /// Adds <typeparamref name="TComponent"/> to the registration.
        /// </summary>
        /// <typeparam name="TComponent">The type of the component to register.</typeparam>
        /// <returns>
        ///     A <see cref="IComponentRegistrationBuilder{TContract, TRegistrationInfo}"/> used to configure the
        ///     component registration.
        /// </returns>
        IComponentRegistrationBuilder<TContract, TypeRegistrationInfo> Add<TComponent>()
            where TComponent : TContract;

        /// <summary>
        /// Adds <typeparamref name="TComponent"/> using factory to the registration.
        /// </summary>
        /// <typeparam name="TComponent">The type of the component to register.</typeparam>
        /// <param name="factory">The delegate used to instantiate the component.</param>
        /// <returns>
        ///     A <see cref="IComponentRegistrationBuilder{TContract, TRegistrationInfo}"/> used to configure the
        ///     component registration.
        /// </returns>
        /// <exception cref="ArgumentNullException">The argument <paramref name="factory"/> is <c>null</c>.</exception>
        IComponentRegistrationBuilder<TContract, DelegateRegistrationInfo> Add<TComponent>(
            Func<IContainer, TComponent> factory)
            where TComponent : TContract;

        /// <summary>
        /// Adds <typeparamref name="TComponent"/> using instance to the registration.
        /// </summary>
        /// <typeparam name="TComponent">The type of the component to register.</typeparam>
        /// <param name="instance">The instance of the component.</param>
        /// <returns>
        ///     A <see cref="IComponentRegistrationBuilder{TContract, TRegistrationInfo}"/> used to configure the
        ///     component registration.
        /// </returns>
        /// <exception cref="ArgumentNullException">The argument <paramref name="instance"/> is <c>null</c>.</exception>
        IComponentRegistrationBuilder<TContract, SingleInstanceRegistrationInfo> Add<TComponent>(
            TComponent instance)
            where TComponent : TContract;

        /// <summary>
        /// Adds <paramref name="componentType"/> using constructor to the registration.
        /// </summary>
        /// <returns>
        ///     A <see cref="IComponentRegistrationBuilder{TContract, TRegistrationInfo}"/> used to configure the
        ///     component registration.
        /// </returns>
        /// <exception cref="ArgumentNullException">The argument <paramref name="componentType"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">The <paramref name="componentType"/> does not implement the contract.</exception>
        IComponentRegistrationBuilder<TContract, TypeRegistrationInfo> Add(Type componentType);
    }
}