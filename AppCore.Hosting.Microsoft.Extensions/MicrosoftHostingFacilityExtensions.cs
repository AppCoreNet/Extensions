// Licensed under the MIT License.
// Copyright (c) 2018-2021 the AppCore .NET project.

using System;
using AppCore.Diagnostics;
using AppCore.Hosting;

// ReSharper disable once CheckNamespace
namespace AppCore.DependencyInjection
{
    /// <summary>
    /// Provides extension methods to register Microsoft.Extensions.Hosting facility extension.
    /// </summary>
    public static class MicrosoftHostingFacilityExtensions
    {
        /// <summary>
        /// Adds hosting adapter using Microsoft.Extensions.
        /// </summary>
        /// <param name="facility">The <see cref="HostingFacility"/>.</param>
        /// <param name="configure">The configuration delegate.</param>
        /// <returns>The <see cref="HostingFacility"/>.</returns>
        public static HostingFacility UseMicrosoftExtensions(
            this HostingFacility facility,
            Action<MicrosoftHostingExtension> configure = null)
        {
            Ensure.Arg.NotNull(facility, nameof(facility));

            facility.AddExtension(configure);
            return facility;
        }
    }
}
