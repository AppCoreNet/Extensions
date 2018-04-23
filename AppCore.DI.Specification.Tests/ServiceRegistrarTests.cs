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
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace AppCore.DependencyInjection
{
    public abstract class ServiceRegistrarTests
    {
        public abstract IServiceRegistrar Registrar { get; }

        protected abstract IServiceProvider BuildServiceProvider();

        [Theory]
        [InlineData(ServiceLifetime.Transient)]
        [InlineData(ServiceLifetime.Scoped)]
        [InlineData(ServiceLifetime.Singleton)]
        public void RegisterWithFactory(ServiceLifetime lifetime)
        {
            var registration = ServiceRegistration.Create(
                typeof(IMyService),
                sp => new MyService(),
                lifetime);

            Registrar.Register(registration);

            var services = (IEnumerable<IMyService>) BuildServiceProvider()
                .GetService(typeof(IEnumerable<IMyService>));

            services.Should()
                    .HaveCount(1);

            services.Should()
                    .AllBeOfType<MyService>();
        }

        [Theory]
        [InlineData(ServiceLifetime.Transient)]
        [InlineData(ServiceLifetime.Scoped)]
        [InlineData(ServiceLifetime.Singleton)]
        public void RegisterOnlyOnceWithFactory(ServiceLifetime lifetime)
        {
            var registration = ServiceRegistration.Create(
                typeof(IMyService),
                sp => new MyService(),
                lifetime);

            Registrar.Register(registration);

            var registration2 = ServiceRegistration.Create(
                typeof(IMyService),
                sp => new MyService2(),
                lifetime,
                ServiceRegistrationFlags.SkipIfRegistered);

            Registrar.Register(registration2);

            var services = (IEnumerable<IMyService>)BuildServiceProvider()
                .GetService(typeof(IEnumerable<IMyService>));

            services.Should()
                    .HaveCount(1);

            services.Should()
                    .AllBeOfType<MyService>();
        }

        [Theory]
        [InlineData(ServiceLifetime.Transient)]
        [InlineData(ServiceLifetime.Scoped)]
        [InlineData(ServiceLifetime.Singleton)]
        public void RegisterOnlyOnceEnumerableWithFactory(ServiceLifetime lifetime)
        {
            var registration = ServiceRegistration.Create(
                typeof(IMyService),
                sp => new MyService(),
                lifetime);

            Registrar.Register(registration);

            var registration2 = ServiceRegistration.Create(
                typeof(IMyService),
                sp => new MyService(),
                lifetime,
                ServiceRegistrationFlags.SkipIfRegistered | ServiceRegistrationFlags.Enumerable);

            Registrar.Register(registration2);

            var services = (IEnumerable<IMyService>)BuildServiceProvider()
                .GetService(typeof(IEnumerable<IMyService>));

            services.Should()
                    .HaveCount(1);

            services.Should()
                    .AllBeOfType<MyService>();
        }

        [Theory]
        [InlineData(ServiceLifetime.Transient)]
        [InlineData(ServiceLifetime.Scoped)]
        [InlineData(ServiceLifetime.Singleton)]
        public void RegisterMultipleEnumerableWithFactory(ServiceLifetime lifetime)
        {
            var registration = ServiceRegistration.Create(
                typeof(IMyService),
                sp => new MyService(),
                lifetime);

            Registrar.Register(registration);

            var registration2 = ServiceRegistration.Create(
                typeof(IMyService),
                sp => new MyService2(),
                lifetime,
                ServiceRegistrationFlags.SkipIfRegistered | ServiceRegistrationFlags.Enumerable);

            Registrar.Register(registration2);

            var services = (IEnumerable<IMyService>)BuildServiceProvider()
                .GetService(typeof(IEnumerable<IMyService>));

            services.Should()
                    .HaveCount(2);

            services.Select(s => s.GetType())
                    .ShouldBeEquivalentTo(
                        new[]
                        {
                            typeof(MyService),
                            typeof(MyService2)
                        },
                        o => o.WithStrictOrdering());
        }

        [Theory]
        [InlineData(ServiceLifetime.Transient)]
        [InlineData(ServiceLifetime.Scoped)]
        [InlineData(ServiceLifetime.Singleton)]
        public void RegisterWithType(ServiceLifetime lifetime)
        {
            var registration = ServiceRegistration.Create(
                typeof(IMyService),
                typeof(MyService),
                lifetime);

            Registrar.Register(registration);

            var services = (IEnumerable<IMyService>)BuildServiceProvider()
                .GetService(typeof(IEnumerable<IMyService>));

            services.Should()
                    .HaveCount(1);

            services.Should()
                    .AllBeOfType<MyService>();
        }

        [Theory]
        [InlineData(ServiceLifetime.Transient)]
        [InlineData(ServiceLifetime.Scoped)]
        [InlineData(ServiceLifetime.Singleton)]
        public void RegisterOnlyOnceWithType(ServiceLifetime lifetime)
        {
            var registration = ServiceRegistration.Create(
                typeof(IMyService),
                typeof(MyService),
                lifetime);

            Registrar.Register(registration);

            var registration2 = ServiceRegistration.Create(
                typeof(IMyService),
                typeof(MyService2),
                lifetime,
                ServiceRegistrationFlags.SkipIfRegistered);

            Registrar.Register(registration2);

            var services = (IEnumerable<IMyService>)BuildServiceProvider()
                .GetService(typeof(IEnumerable<IMyService>));

            services.Should()
                    .HaveCount(1);

            services.Should()
                    .AllBeOfType<MyService>();
        }

        [Theory]
        [InlineData(ServiceLifetime.Transient)]
        [InlineData(ServiceLifetime.Scoped)]
        [InlineData(ServiceLifetime.Singleton)]
        public void RegisterOnlyOnceEnumerableWithType(ServiceLifetime lifetime)
        {
            var registration = ServiceRegistration.Create(
                typeof(IMyService),
                typeof(MyService),
                lifetime);

            Registrar.Register(registration);

            var registration2 = ServiceRegistration.Create(
                typeof(IMyService),
                typeof(MyService),
                lifetime,
                ServiceRegistrationFlags.SkipIfRegistered | ServiceRegistrationFlags.Enumerable);

            Registrar.Register(registration2);

            var services = (IEnumerable<IMyService>)BuildServiceProvider()
                .GetService(typeof(IEnumerable<IMyService>));

            services.Should()
                    .HaveCount(1);

            services.Should()
                    .AllBeOfType<MyService>();
        }

        [Theory]
        [InlineData(ServiceLifetime.Transient)]
        [InlineData(ServiceLifetime.Scoped)]
        [InlineData(ServiceLifetime.Singleton)]
        public void RegisterMultipleEnumerableWithType(ServiceLifetime lifetime)
        {
            var registration = ServiceRegistration.Create(
                typeof(IMyService),
                typeof(MyService),
                lifetime);

            Registrar.Register(registration);

            var registration2 = ServiceRegistration.Create(
                typeof(IMyService),
                typeof(MyService2),
                lifetime,
                ServiceRegistrationFlags.SkipIfRegistered | ServiceRegistrationFlags.Enumerable);

            Registrar.Register(registration2);

            var services = (IEnumerable<IMyService>)BuildServiceProvider()
                .GetService(typeof(IEnumerable<IMyService>));

            services.Should()
                    .HaveCount(2);

            services.Select(s => s.GetType())
                    .ShouldBeEquivalentTo(
                        new[]
                        {
                            typeof(MyService),
                            typeof(MyService2)
                        },
                        o => o.WithStrictOrdering());
        }

        [Fact]
        public void RegisterWithInstance()
        {
            var registration = ServiceRegistration.Create(
                typeof(IMyService),
                new MyService());

            Registrar.Register(registration);

            var services = (IEnumerable<IMyService>)BuildServiceProvider()
                .GetService(typeof(IEnumerable<IMyService>));

            services.Should()
                    .HaveCount(1);

            services.Should()
                    .AllBeOfType<MyService>();
        }

        [Fact]
        public void RegisterOnlyOnceWithInstance()
        {
            var registration = ServiceRegistration.Create(
                typeof(IMyService),
                new MyService());

            Registrar.Register(registration);

            var registration2 = ServiceRegistration.Create(
                typeof(IMyService),
                new MyService2(),
                ServiceRegistrationFlags.SkipIfRegistered);

            Registrar.Register(registration2);

            var services = (IEnumerable<IMyService>)BuildServiceProvider()
                .GetService(typeof(IEnumerable<IMyService>));

            services.Should()
                    .HaveCount(1);

            services.Should()
                    .AllBeOfType<MyService>();
        }

        [Fact]
        public void RegisterOnlyOnceEnumerableWithInstance()
        {
            var registration = ServiceRegistration.Create(
                typeof(IMyService),
                new MyService());

            Registrar.Register(registration);

            var registration2 = ServiceRegistration.Create(
                typeof(IMyService),
                new MyService(),
                ServiceRegistrationFlags.SkipIfRegistered | ServiceRegistrationFlags.Enumerable);

            Registrar.Register(registration2);

            var services = (IEnumerable<IMyService>)BuildServiceProvider()
                .GetService(typeof(IEnumerable<IMyService>));

            services.Should()
                    .HaveCount(1);

            services.Should()
                    .AllBeOfType<MyService>();
        }

        [Fact]
        public void RegisterMultipleEnumerableWithInstance()
        {
            var registration = ServiceRegistration.Create(
                typeof(IMyService),
                new MyService());

            Registrar.Register(registration);

            var registration2 = ServiceRegistration.Create(
                typeof(IMyService),
                new MyService2(),
                ServiceRegistrationFlags.SkipIfRegistered | ServiceRegistrationFlags.Enumerable);

            Registrar.Register(registration2);

            var services = (IEnumerable<IMyService>)BuildServiceProvider()
                .GetService(typeof(IEnumerable<IMyService>));

            services.Should()
                    .HaveCount(2);

            services.Select(s => s.GetType())
                    .ShouldBeEquivalentTo(
                        new[]
                        {
                            typeof(MyService),
                            typeof(MyService2)
                        },
                        o => o.WithStrictOrdering());
        }
    }
}