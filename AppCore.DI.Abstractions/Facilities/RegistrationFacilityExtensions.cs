// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System;
using AppCore.DependencyInjection.Builder;
using AppCore.Diagnostics;

namespace AppCore.DependencyInjection.Facilities
{
    /// <summary>
    /// Provides a <see cref="IFacilityExtension{TFacility}"/> which registers components via callback.
    /// </summary>
    /// <typeparam name="TFacility">The type of the facility.</typeparam>
    public sealed class RegistrationFacilityExtension<TFacility> : FacilityExtension<TFacility>
        where TFacility : IFacility
    {
        private readonly Type _contractType;
        private readonly Action<IRegistrationBuilder, TFacility> _register;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrationFacilityExtension{TFacility}"/> class.
        /// </summary>
        /// <param name="contractType">The contract type of the components to register.</param>
        /// <param name="register">The callback invoked to register component implementations.</param>
        public RegistrationFacilityExtension(Type contractType, Action<IRegistrationBuilder, TFacility> register)
        {
            Ensure.Arg.NotNull(contractType, nameof(contractType));
            Ensure.Arg.NotNull(register, nameof(register));

            _contractType = contractType;
            _register = register;
        }

        /// <inheritdoc />
        protected override void RegisterComponents(IComponentRegistry registry, TFacility facility)
        {
            _register(registry.Register(_contractType), facility);
        }
    }

    /// <summary>
    /// Provides a <see cref="IFacilityExtension{TFacility}"/> which registers components via callback.
    /// </summary>
    /// <typeparam name="TFacility">The type of the facility.</typeparam>
    /// <typeparam name="TContract">The contract type of the components to register.</typeparam>
    public sealed class RegistrationFacilityExtension<TFacility, TContract> : FacilityExtension<TFacility>
        where TFacility : IFacility
    {
        private readonly Action<IRegistrationBuilder<TContract>, TFacility> _register;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrationFacilityExtension{TFacility}"/> class.
        /// </summary>
        /// <param name="register">The callback invoked to register component implementations.</param>
        public RegistrationFacilityExtension(Action<IRegistrationBuilder<TContract>, TFacility> register)
        {
            Ensure.Arg.NotNull(register, nameof(register));

            _register = register;
        }

        /// <inheritdoc />
        protected override void RegisterComponents(IComponentRegistry registry, TFacility facility)
        {
            _register(registry.Register<TContract>(), facility);
        }
    }
}