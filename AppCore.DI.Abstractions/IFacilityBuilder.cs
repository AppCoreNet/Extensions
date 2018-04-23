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
    /// Represents a type to register facilities.
    /// </summary>
    public interface IFacilityBuilder
    {
        /// <summary>
        /// The <see cref="IFacility"/> which is being registered.
        /// </summary>
        IFacility Facility { get; }

        /// <summary>
        /// The <see cref="IServiceRegistrar"/> where services are registered.
        /// </summary>
        IServiceRegistrar Registrar { get; }

        /// <summary>
        /// Invoked to register services of the facility.
        /// </summary>
        /// <param name="registrar">The <see cref="IServiceRegistrar"/>.</param>
        void RegisterServices(IServiceRegistrar registrar);
    }

    /// <summary>
    /// Represents a type to register facilities.
    /// </summary>
    /// <typeparam name="TFacility">The type of the facility.</typeparam>
    /// <seealso cref="IFacility"/>
    /// <seealso cref="IFacilityExtension{TFacility}"/>
    public interface IFacilityBuilder<out TFacility> : IFacilityBuilder
        where TFacility : IFacility
    {
        /// <summary>
        /// The <see cref="IFacility"/> which is being registered.
        /// </summary>
        new TFacility Facility { get; }

        /// <summary>
        /// Add registration of a facility extension.
        /// </summary>
        /// <param name="extension">The <see cref="IFacilityExtension{TFacility}"/> to use.</param>
        void UseExtension(IFacilityExtension<TFacility> extension);
    }
}