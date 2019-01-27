using System;
using AppCore.DependencyInjection;
using AppCore.DependencyInjection.Autofac;
using AppCore.DependencyInjection.Facilities;
using AppCore.Diagnostics;

// ReSharper disable once CheckNamespace
namespace Autofac
{
    /// <summary>
    /// Provides extensions methods to register facilities with a <see cref="ContainerBuilder"/>.
    /// </summary>
    public static class FacilityRegistrationExtensions
    {
        /// <summary>
        /// Register the specified <typeparamref name="TFacility"/> with the service collection.
        /// </summary>
        /// <typeparam name="TFacility">The type of the <see cref="IFacility"/> to register.</typeparam>
        /// <param name="builder">The <see cref="ContainerBuilder"/> where to register the facility.</param>
        /// <param name="configure">Optional delegate to configure the facility.</param>
        /// <returns>The <see cref="ContainerBuilder"/>.</returns>
        public static ContainerBuilder RegisterFacility<TFacility>(
            this ContainerBuilder builder,
            Action<IFacilityBuilder<TFacility>> configure = null)
            where TFacility : IFacility, new()
        {
            Ensure.Arg.NotNull(builder, nameof(builder));

            var registry = new AutofacComponentRegistry();
            IFacilityBuilder<TFacility> facilityBuilder = registry.RegisterFacility<TFacility>();
            configure?.Invoke(facilityBuilder);
            registry.RegisterComponents(builder);
            return builder;
        }

        /// <summary>
        /// Register the specified <paramref name="facility"/> with the service collection.
        /// </summary>
        /// <param name="facility">The <see cref="IFacility"/> to register.</param>
        /// <param name="builder">The <see cref="ContainerBuilder"/> where to register the facility.</param>
        /// <param name="configure">Optional delegate to configure the facility.</param>
        /// <returns>The <see cref="ContainerBuilder"/>.</returns>
        public static ContainerBuilder RegisterFacility(
            this ContainerBuilder builder,
            IFacility facility,
            Action<IFacilityBuilder> configure = null)
        {
            Ensure.Arg.NotNull(builder, nameof(builder));

            var registry = new AutofacComponentRegistry();
            IFacilityBuilder facilityBuilder = registry.RegisterFacility(facility);
            configure?.Invoke(facilityBuilder);
            registry.RegisterComponents(builder);
            return builder;
        }
    }
}
