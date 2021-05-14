// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Reflection;
using System.Runtime.ExceptionServices;
using AppCore.Diagnostics;

namespace AppCore.DependencyInjection
{
    /// <inheritdoc />
    public class Activator : IActivator
    {
        private readonly IContainer _container;

        /// <summary>
        /// Initializes a new instance of the <see cref="Activator"/> class.
        /// </summary>
        /// <param name="container">The <see cref="IContainer"/> used to resolve services.</param>
        public Activator(IContainer container)
        {
            Ensure.Arg.NotNull(container, nameof(container));
            _container = container;
        }

        /// <inheritdoc />
        public object CreateInstance(Type instanceType, params object[] parameters)
        {
            int bestLength = -1;

            ConstructorMatcher bestMatcher = default;

            if (!instanceType.IsAbstract)
            {
                foreach (ConstructorInfo constructor in instanceType.GetConstructors())
                {
                    var matcher = new ConstructorMatcher(constructor);
                    int length = matcher.Match(parameters);

                    if (bestLength < length)
                    {
                        bestLength = length;
                        bestMatcher = matcher;
                    }
                }
            }

            if (bestLength == -1)
            {
                string message = $"A suitable constructor for type '{instanceType}' could not be located. Ensure the type is concrete and all parameters of a public constructor are either registered as services or passed as arguments. Also ensure no extraneous arguments are provided.";
                throw new InvalidOperationException(message);
            }

            return bestMatcher.CreateInstance(_container);
        }

        /// <inheritdoc />
        public object ResolveOrCreateInstance(Type type)
        {
            return _container.ResolveOptional(type) ?? CreateInstance(type);
        }

        private struct ConstructorMatcher
        {
            private readonly ConstructorInfo _constructor;
            private readonly ParameterInfo[] _parameters;
            private readonly object[] _parameterValues;

            public ConstructorMatcher(ConstructorInfo constructor)
            {
                _constructor = constructor;
                _parameters = _constructor.GetParameters();
                _parameterValues = new object[_parameters.Length];
            }

            public int Match(object[] givenParameters)
            {
                int applyIndexStart = 0;
                int applyExactLength = 0;
                for (int givenIndex = 0; givenIndex != givenParameters.Length; givenIndex++)
                {
                    Type givenType = givenParameters[givenIndex].GetType();
                    bool givenMatched = false;

                    for (int applyIndex = applyIndexStart; givenMatched == false && applyIndex != _parameters.Length; ++applyIndex)
                    {
                        if (_parameterValues[applyIndex] == null &&
                            _parameters[applyIndex].ParameterType.IsAssignableFrom(givenType))
                        {
                            givenMatched = true;
                            _parameterValues[applyIndex] = givenParameters[givenIndex];
                            if (applyIndexStart == applyIndex)
                            {
                                applyIndexStart++;
                                if (applyIndex == givenIndex)
                                {
                                    applyExactLength = applyIndex;
                                }
                            }
                        }
                    }

                    if (givenMatched == false)
                    {
                        return -1;
                    }
                }
                return applyExactLength;
            }

            public object CreateInstance(IContainer container)
            {
                for (int index = 0; index != _parameters.Length; index++)
                {
                    if (_parameterValues[index] == null)
                    {
                        object value = container.ResolveOptional(_parameters[index].ParameterType);
                        if (value == null)
                        {
                            if (!_parameters[index].HasDefaultValue)
                            {
                                throw new InvalidOperationException($"Unable to resolve service for type '{_parameters[index].ParameterType}' while attempting to activate '{_constructor.DeclaringType}'.");
                            }
                            else
                            {
                                _parameterValues[index] = _parameters[index].DefaultValue;
                            }
                        }
                        else
                        {
                            _parameterValues[index] = value;
                        }
                    }
                }

#if NETCOREAPP
                return _constructor.Invoke(BindingFlags.DoNotWrapExceptions, binder: null, parameters: _parameterValues, culture: null);
#else
                try
                {
                    return _constructor.Invoke(_parameterValues);
                }
                catch (TargetInvocationException ex) when (ex.InnerException != null)
                {
                    ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                    // The above line will always throw, but the compiler requires we throw explicitly.
                    throw;
                }
#endif
            }
        }
    }
}