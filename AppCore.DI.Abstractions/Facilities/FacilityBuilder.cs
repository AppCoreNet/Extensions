// Licensed under the MIT License.
// Copyright (c) 2018,2019 the AppCore .NET project.

using System;
using System.Collections.Generic;
using AppCore.Diagnostics;

namespace AppCore.DependencyInjection.Facilities
{
    internal sealed class FacilityBuilder<TFacility> : IFacilityBuilder<TFacility>
        where TFacility : IFacility, new()
    {
        private readonly List<Action<TFacility>> _callbacks = new List<Action<TFacility>>();

        public void Configure(Action<TFacility> configure)
        {
            Ensure.Arg.NotNull(configure, nameof(configure));
            _callbacks.Add(configure);
        }

        public TFacility Build()
        {
            var facility = new TFacility();
            foreach (Action<TFacility> callback in _callbacks)
                callback(facility);

            return facility;
        }

        public IFacilityBuilder<TFacility> Add<TExtension>(TExtension extension)
            where TExtension : IFacilityExtension<TFacility>
        {
            Ensure.Arg.NotNull(extension, nameof(extension));

            Configure(facility =>
            {
                facility.Extensions.Add(extension);
            });

            return this;
        }

        public IFacilityBuilder<TFacility> Add<TExtension>(
            Action<IFacilityExtensionBuilder<TFacility, TExtension>> configure)
            where TExtension : IFacilityExtension<TFacility>, new()
        {
            Configure(facility =>
            {
                var builder = new FacilityExtensionBuilder<TFacility, TExtension>();
                configure?.Invoke(builder);

                facility.Extensions.Add(builder.Build(facility));
            });

            return this;
        }
    }
}