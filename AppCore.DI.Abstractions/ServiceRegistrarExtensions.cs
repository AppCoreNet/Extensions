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

namespace AppCore.DependencyInjection
{
    public static class ServiceRegistrarExtensions
    {
        public static IServiceRegistrar AddTransient(
            this IServiceRegistrar registrar,
            Type serviceType,
            Type implementationType)
        {
            registrar.Register(
                ServiceRegistration.Create(
                    serviceType,
                    implementationType,
                    ServiceLifetime.Transient));

            return registrar;
        }

        public static IServiceRegistrar AddTransient<TService, TImplementation>(this IServiceRegistrar registrar)
            where TImplementation : TService
        {
            registrar.Register(
                ServiceRegistration.Create(
                    typeof(TService),
                    typeof(TImplementation),
                    ServiceLifetime.Transient));

            return registrar;
        }

        public static IServiceRegistrar AddTransient<TService, TImplementation>(
            this IServiceRegistrar registrar,
            Func<IServiceProvider, TImplementation> factory)
            where TImplementation : TService
        {
            registrar.Register(
                ServiceRegistration.Create(
                    typeof(TService),
                    factory,
                    ServiceLifetime.Transient));

            return registrar;
        }

        public static IServiceRegistrar TryAddTransient(
            this IServiceRegistrar registrar,
            Type serviceType,
            Type implementationType)
        {
            registrar.Register(
                ServiceRegistration.Create(
                    serviceType,
                    implementationType,
                    ServiceLifetime.Transient,
                    ServiceRegistrationFlags.SkipIfRegistered));

            return registrar;
        }

        public static IServiceRegistrar TryAddTransient<TService, TImplementation>(this IServiceRegistrar registrar)
            where TImplementation : TService
        {
            registrar.Register(
                ServiceRegistration.Create(
                    typeof(TService),
                    typeof(TImplementation),
                    ServiceLifetime.Transient,
                    ServiceRegistrationFlags.SkipIfRegistered));

            return registrar;
        }

        public static IServiceRegistrar TryAddTransient<TService, TImplementation>(
            this IServiceRegistrar registrar,
            Func<IServiceProvider, TImplementation> factory)
            where TImplementation : TService
        {
            registrar.Register(
                ServiceRegistration.Create(
                    typeof(TService),
                    sp => factory,
                    ServiceLifetime.Transient,
                    ServiceRegistrationFlags.SkipIfRegistered));

            return registrar;
        }

        public static IServiceRegistrar AddScoped(
            this IServiceRegistrar registrar,
            Type serviceType,
            Type implementationType)
        {
            registrar.Register(
                ServiceRegistration.Create(
                    serviceType,
                    implementationType,
                    ServiceLifetime.Scoped));

            return registrar;
        }

        public static IServiceRegistrar AddScoped<TService, TImplementation>(this IServiceRegistrar registrar)
            where TImplementation : TService
        {
            registrar.Register(
                ServiceRegistration.Create(
                    typeof(TService),
                    typeof(TImplementation),
                    ServiceLifetime.Scoped));

            return registrar;
        }

        public static IServiceRegistrar AddScoped<TService, TImplementation>(
            this IServiceRegistrar registrar,
            Func<IServiceProvider, TImplementation> factory)
            where TImplementation : TService
        {
            registrar.Register(
                ServiceRegistration.Create(
                    typeof(TService),
                    factory,
                    ServiceLifetime.Scoped));

            return registrar;
        }

        public static IServiceRegistrar TryAddScoped(
            this IServiceRegistrar registrar,
            Type serviceType,
            Type implementationType)
        {
            registrar.Register(
                ServiceRegistration.Create(
                    serviceType,
                    implementationType,
                    ServiceLifetime.Scoped,
                    ServiceRegistrationFlags.SkipIfRegistered));

            return registrar;
        }

        public static IServiceRegistrar TryAddScoped<TService, TImplementation>(this IServiceRegistrar registrar)
            where TImplementation : TService
        {
            registrar.Register(
                ServiceRegistration.Create(
                    typeof(TService),
                    typeof(TImplementation),
                    ServiceLifetime.Scoped,
                    ServiceRegistrationFlags.SkipIfRegistered));

            return registrar;
        }

        public static IServiceRegistrar TryAddScoped<TService, TImplementation >(
            this IServiceRegistrar registrar,
            Func<IServiceProvider, TImplementation> factory)
            where TImplementation : TService
        {
            registrar.Register(
                ServiceRegistration.Create(
                    typeof(TService),
                    factory,
                    ServiceLifetime.Scoped,
                    ServiceRegistrationFlags.SkipIfRegistered));

            return registrar;
        }

        public static IServiceRegistrar AddSingleton(
            this IServiceRegistrar registrar,
            Type serviceType,
            Type implementationType)
        {
            registrar.Register(
                ServiceRegistration.Create(
                    serviceType,
                    implementationType,
                    ServiceLifetime.Singleton));

            return registrar;
        }

        public static IServiceRegistrar AddSingleton<TService, TImplementation>(this IServiceRegistrar registrar)
            where TImplementation : TService
        {
            registrar.Register(
                ServiceRegistration.Create(
                    typeof(TService),
                    typeof(TImplementation),
                    ServiceLifetime.Singleton));

            return registrar;
        }

        public static IServiceRegistrar AddSingleton<TService, TImplementation>(
            this IServiceRegistrar registrar,
            Func<IServiceProvider, TImplementation> factory)
            where TImplementation : TService
        {
            registrar.Register(
                ServiceRegistration.Create(
                    typeof(TService),
                    factory,
                    ServiceLifetime.Singleton));

            return registrar;
        }

        public static IServiceRegistrar AddSingleton<TService>(this IServiceRegistrar registrar, TService instance)
        {
            registrar.Register(
                ServiceRegistration.Create(
                    typeof(TService),
                    instance));

            return registrar;
        }

        public static IServiceRegistrar TryAddSingleton(
            this IServiceRegistrar registrar,
            Type serviceType,
            Type implementationType)
        {
            registrar.Register(
                ServiceRegistration.Create(
                    serviceType,
                    implementationType,
                    ServiceLifetime.Singleton,
                    ServiceRegistrationFlags.SkipIfRegistered));

            return registrar;
        }

        public static IServiceRegistrar TryAddSingleton<TService, TImplementation>(this IServiceRegistrar registrar)
            where TImplementation : TService
        {
            registrar.Register(
                ServiceRegistration.Create(
                    typeof(TService),
                    typeof(TImplementation),
                    ServiceLifetime.Singleton,
                    ServiceRegistrationFlags.SkipIfRegistered));

            return registrar;
        }

        public static IServiceRegistrar TryAddSingleton<TService, TImplementation>(
            this IServiceRegistrar registrar,
            Func<IServiceProvider, TImplementation> factory)
            where TImplementation : TService
        {
            registrar.Register(
                ServiceRegistration.Create(
                    typeof(TService),
                    factory,
                    ServiceLifetime.Singleton,
                    ServiceRegistrationFlags.SkipIfRegistered));

            return registrar;
        }

        public static IServiceRegistrar TryAddSingleton<TService>(this IServiceRegistrar registrar, TService instance)
        {
            registrar.Register(
                ServiceRegistration.Create(
                    typeof(TService),
                    instance,
                    ServiceRegistrationFlags.SkipIfRegistered));

            return registrar;
        }

        public static IServiceRegistrar AddFacility(
            this IServiceRegistrar registrar,
            IFacility facility,
            Action<FacilityBuilder> configure = null)
        {
            var builder = new FacilityBuilder(facility);
            configure?.Invoke(builder);
            ((IFacilityBuilder) builder).RegisterServices(registrar);
            return registrar;
        }

        public static IServiceRegistrar AddFacility<TFacility>(
            this IServiceRegistrar registrar,
            Action<FacilityBuilder<TFacility>> configure = null)
            where TFacility : IFacility, new()
        {
            var builder = new FacilityBuilder<TFacility>(new TFacility());
            configure?.Invoke(builder);
            ((IFacilityBuilder)builder).RegisterServices(registrar);
            return registrar;
        }
    }
}