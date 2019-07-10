// Licensed under the MIT License.
// Copyright (c) 2018,2019 the AppCore .NET project.

using System;
using System.Collections.Generic;
using AppCore.Diagnostics;

namespace AppCore.DependencyInjection.Facilities
{
    internal sealed class FacilityExtensionBuilder<TFacility, TExtension> : IFacilityExtensionBuilder<TFacility, TExtension>
        where TFacility : IFacility
        where TExtension : IFacilityExtension<TFacility>, new()
    {
        private readonly List<Action<TFacility, TExtension>> _callbacks = new List<Action<TFacility, TExtension>>();

        public void Configure(Action<TFacility, TExtension> configure)
        {
            Ensure.Arg.NotNull(configure, nameof(configure));
            _callbacks.Add(configure);
        }

        public TExtension Build(TFacility facility)
        {
            var extension = new TExtension();

            foreach (Action<TFacility, TExtension> callback in _callbacks)
            {
                callback(facility, extension);
            }

            return extension;
        }
    }
}