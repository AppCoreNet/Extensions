// Copyright 2018 the AppCore project.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation
// the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and
// to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions
// of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
// BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using AppCore.Diagnostics;

namespace AppCore.DependencyInjection
{
    public static class ComponentRegistryExtensions
    {
        public static IComponentRegistry AddTransient(
            this IComponentRegistry registrar,
            Type serviceType,
            Type implementationType)
        {
            registrar.Register(
                ComponentRegistration.Create(
                    serviceType,
                    implementationType,
                    ComponentLifetime.Transient));

            return registrar;
        }

        public static IComponentRegistry AddTransient<TService, TImplementation>(this IComponentRegistry registrar)
            where TImplementation : TService
        {
            registrar.Register(
                ComponentRegistration.Create(
                    typeof(TService),
                    typeof(TImplementation),
                    ComponentLifetime.Transient));

            return registrar;
        }

        public static IComponentRegistry AddTransient<TService, TImplementation>(
            this IComponentRegistry registrar,
            Func<IContainer, TImplementation> factory)
            where TImplementation : TService
        {
            registrar.Register(
                ComponentRegistration.Create(
                    typeof(TService),
                    factory,
                    ComponentLifetime.Transient));

            return registrar;
        }

        public static IComponentRegistry TryAddTransient(
            this IComponentRegistry registrar,
            Type serviceType,
            Type implementationType)
        {
            registrar.Register(
                ComponentRegistration.Create(
                    serviceType,
                    implementationType,
                    ComponentLifetime.Transient,
                    ComponentRegistrationFlags.SkipIfRegistered));

            return registrar;
        }

        public static IComponentRegistry TryAddTransient<TService, TImplementation>(this IComponentRegistry registrar)
            where TImplementation : TService
        {
            registrar.Register(
                ComponentRegistration.Create(
                    typeof(TService),
                    typeof(TImplementation),
                    ComponentLifetime.Transient,
                    ComponentRegistrationFlags.SkipIfRegistered));

            return registrar;
        }

        public static IComponentRegistry TryAddTransient<TService, TImplementation>(
            this IComponentRegistry registrar,
            Func<IServiceProvider, TImplementation> factory)
            where TImplementation : TService
        {
            registrar.Register(
                ComponentRegistration.Create(
                    typeof(TService),
                    sp => factory,
                    ComponentLifetime.Transient,
                    ComponentRegistrationFlags.SkipIfRegistered));

            return registrar;
        }

        public static IComponentRegistry AddScoped(
            this IComponentRegistry registrar,
            Type serviceType,
            Type implementationType)
        {
            registrar.Register(
                ComponentRegistration.Create(
                    serviceType,
                    implementationType,
                    ComponentLifetime.Scoped));

            return registrar;
        }

        public static IComponentRegistry AddScoped<TService, TImplementation>(this IComponentRegistry registrar)
            where TImplementation : TService
        {
            registrar.Register(
                ComponentRegistration.Create(
                    typeof(TService),
                    typeof(TImplementation),
                    ComponentLifetime.Scoped));

            return registrar;
        }

        public static IComponentRegistry AddScoped<TService, TImplementation>(
            this IComponentRegistry registrar,
            Func<IContainer, TImplementation> factory)
            where TImplementation : TService
        {
            registrar.Register(
                ComponentRegistration.Create(
                    typeof(TService),
                    factory,
                    ComponentLifetime.Scoped));

            return registrar;
        }

        public static IComponentRegistry TryAddScoped(
            this IComponentRegistry registrar,
            Type serviceType,
            Type implementationType)
        {
            registrar.Register(
                ComponentRegistration.Create(
                    serviceType,
                    implementationType,
                    ComponentLifetime.Scoped,
                    ComponentRegistrationFlags.SkipIfRegistered));

            return registrar;
        }

        public static IComponentRegistry TryAddScoped<TService, TImplementation>(this IComponentRegistry registrar)
            where TImplementation : TService
        {
            registrar.Register(
                ComponentRegistration.Create(
                    typeof(TService),
                    typeof(TImplementation),
                    ComponentLifetime.Scoped,
                    ComponentRegistrationFlags.SkipIfRegistered));

            return registrar;
        }

        public static IComponentRegistry TryAddScoped<TService, TImplementation >(
            this IComponentRegistry registrar,
            Func<IContainer, TImplementation> factory)
            where TImplementation : TService
        {
            registrar.Register(
                ComponentRegistration.Create(
                    typeof(TService),
                    factory,
                    ComponentLifetime.Scoped,
                    ComponentRegistrationFlags.SkipIfRegistered));

            return registrar;
        }

        public static IComponentRegistry AddSingleton(
            this IComponentRegistry registrar,
            Type serviceType,
            Type implementationType)
        {
            registrar.Register(
                ComponentRegistration.Create(
                    serviceType,
                    implementationType,
                    ComponentLifetime.Singleton));

            return registrar;
        }

        public static IComponentRegistry AddSingleton<TService, TImplementation>(this IComponentRegistry registrar)
            where TImplementation : TService
        {
            registrar.Register(
                ComponentRegistration.Create(
                    typeof(TService),
                    typeof(TImplementation),
                    ComponentLifetime.Singleton));

            return registrar;
        }

        public static IComponentRegistry AddSingleton<TService, TImplementation>(
            this IComponentRegistry registrar,
            Func<IContainer, TImplementation> factory)
            where TImplementation : TService
        {
            registrar.Register(
                ComponentRegistration.Create(
                    typeof(TService),
                    factory,
                    ComponentLifetime.Singleton));

            return registrar;
        }

        public static IComponentRegistry AddSingleton<TService>(this IComponentRegistry registrar, TService instance)
        {
            registrar.Register(
                ComponentRegistration.Create(
                    typeof(TService),
                    instance));

            return registrar;
        }

        public static IComponentRegistry TryAddSingleton(
            this IComponentRegistry registrar,
            Type serviceType,
            Type implementationType)
        {
            registrar.Register(
                ComponentRegistration.Create(
                    serviceType,
                    implementationType,
                    ComponentLifetime.Singleton,
                    ComponentRegistrationFlags.SkipIfRegistered));

            return registrar;
        }

        public static IComponentRegistry TryAddSingleton<TService, TImplementation>(this IComponentRegistry registrar)
            where TImplementation : TService
        {
            registrar.Register(
                ComponentRegistration.Create(
                    typeof(TService),
                    typeof(TImplementation),
                    ComponentLifetime.Singleton,
                    ComponentRegistrationFlags.SkipIfRegistered));

            return registrar;
        }

        public static IComponentRegistry TryAddSingleton<TService, TImplementation>(
            this IComponentRegistry registrar,
            Func<IContainer, TImplementation> factory)
            where TImplementation : TService
        {
            registrar.Register(
                ComponentRegistration.Create(
                    typeof(TService),
                    factory,
                    ComponentLifetime.Singleton,
                    ComponentRegistrationFlags.SkipIfRegistered));

            return registrar;
        }

        public static IComponentRegistry TryAddSingleton<TService>(this IComponentRegistry registrar, TService instance)
        {
            registrar.Register(
                ComponentRegistration.Create(
                    typeof(TService),
                    instance,
                    ComponentRegistrationFlags.SkipIfRegistered));

            return registrar;
        }

        /// <summary>
        /// Registers a facility with the dependency injection container.
        /// </summary>
        /// <param name="registry">The <see cref="IComponentRegistry"/> where to register components.</param>
        /// <param name="facility">The <see cref="IFacility"/> which is registered.</param>
        /// <param name="configure">Delegate to configure the facility.</param>
        /// <returns>The <see cref="IComponentRegistry"/>.</returns>
        public static IComponentRegistry RegisterFacility(
            this IComponentRegistry registry,
            IFacility facility,
            Action<FacilityBuilder> configure = null)
        {
            Ensure.Arg.NotNull(registry, nameof(registry));
            Ensure.Arg.NotNull(facility, nameof(facility));

            var builder = new FacilityBuilder(facility);
            configure?.Invoke(builder);
            ((IFacilityBuilder) builder).RegisterComponents(registry);
            return registry;
        }

        /// <summary>
        /// Registers a facility with the dependency injection container.
        /// </summary>
        /// <typeparam name="TFacility">The type of the facility.</typeparam>
        /// <param name="registry">The <see cref="IComponentRegistry"/> where to register components.</param>
        /// <param name="configure">Delegate to configure the facility.</param>
        /// <returns>The <see cref="IComponentRegistry"/>.</returns>
        public static IComponentRegistry RegisterFacility<TFacility>(
            this IComponentRegistry registry,
            Action<FacilityBuilder<TFacility>> configure = null)
            where TFacility : IFacility, new()
        {
            Ensure.Arg.NotNull(registry, nameof(registry));

            var builder = new FacilityBuilder<TFacility>(new TFacility());
            configure?.Invoke(builder);
            ((IFacilityBuilder)builder).RegisterComponents(registry);
            return registry;
        }
    }
}