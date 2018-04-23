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
using Autofac;

namespace AppCore.DependencyInjection
{
    /// <summary>
    /// Autofac based <see cref="IServiceProvider"/> implementation.
    /// </summary>
    public class AutofacServiceProvider : IServiceProvider
    {
        private readonly IComponentContext _context;

        public AutofacServiceProvider(IComponentContext context)
        {
            Ensure.Arg.NotNull(context, nameof(context));
            _context = context;
        }

        /// <inheritdoc />
        public object GetService(Type serviceType)
        {
            Ensure.Arg.NotNull(serviceType, nameof(serviceType));
            return _context.ResolveOptional(serviceType);
        }
    }
}