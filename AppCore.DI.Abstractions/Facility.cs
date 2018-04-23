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
    /// <summary>
    /// Abstract base class for the <see cref="IFacility"/> type.
    /// </summary>
    /// <seealso cref="IFacility"/>
    public abstract class Facility : IFacility
    {
        private readonly Action<IServiceRegistrar> _registrationCallback;

        /// <summary>
        /// Initializes a new instance of the <see cref="Facility"/> class.
        /// </summary>
        /// <param name="registrationCallback">
        ///     The <see cref="Action{T}"/> which is invoked when facility services should be registered.
        /// </param>
        protected Facility(Action<IServiceRegistrar> registrationCallback)
        {
            Ensure.Arg.NotNull(registrationCallback, nameof(registrationCallback));
            _registrationCallback = registrationCallback;
        }

        void IFacility.RegisterServices(IServiceRegistrar registrar)
        {
            _registrationCallback(registrar);
        }
    }
}