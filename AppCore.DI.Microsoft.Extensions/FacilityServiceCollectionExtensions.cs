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
using AppCore.DependencyInjection;
using AppCore.DependencyInjection.Microsoft.Extensions;
using AppCore.Diagnostics;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Provides extension methods for registering facilities with a <see cref="IServiceCollection"/>.
    /// </summary>
    public static class FacilityServiceCollectionExtensions
    {
        /// <summary>
        /// Registers a facility with the given <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> where to register.</param>
        /// <param name="facility">The <see cref="IFacility"/> to register.</param>
        /// <param name="configure">Delegate invoked to configure the facility.</param>
        public static IServiceCollection AddFacility(
            this IServiceCollection services,
            IFacility facility,
            Action<FacilityBuilder> configure = null)
        {
            Ensure.Arg.NotNull(services, nameof(services));
            Ensure.Arg.NotNull(facility, nameof(facility));

            var registry = new MicrosoftComponentRegistry(services);
            registry.RegisterFacility(facility, configure);
            return services;
        }

        /// <summary>
        /// Registers a facility with the given <see cref="IServiceCollection"/>.
        /// </summary>
        /// <typeparam name="TFacility">The type of the <see cref="IFacility"/> to register.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/> where to register.</param>
        /// <param name="configure">Delegate invoked to configure the facility.</param>
        public static IServiceCollection AddFacility<TFacility>(
            this IServiceCollection services,
            Action<FacilityBuilder<TFacility>> configure = null)
            where TFacility : IFacility, new()
        {
            Ensure.Arg.NotNull(services, nameof(services));

            var registry = new MicrosoftComponentRegistry(services);
            registry.RegisterFacility(configure);
            return services;
        }
    }
}