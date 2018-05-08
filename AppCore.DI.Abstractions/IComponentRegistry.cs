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

namespace AppCore.DependencyInjection
{
    /// <summary>
    /// Represents a type used to register components with a dependency injection container.
    /// </summary>
    /// <seealso cref="ComponentRegistration"/>
    /// <seealso cref="ComponentAssemblyRegistration"/>
    public interface IComponentRegistry
    {
        /// <summary>
        /// Registers a component with the dependency injection container.
        /// </summary>
        /// <param name="registration">The <see cref="ComponentRegistration"/> describing the component to register.</param>
        void Register(ComponentRegistration registration);

        /// <summary>
        /// Registers components from a set of assemblies with the dependency injection container.
        /// </summary>
        /// <param name="registration">The <see cref="ComponentAssemblyRegistration"/> describing the components to register.</param>
        void RegisterAssembly(ComponentAssemblyRegistration registration);
    }
}