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

using System.Linq;
using FluentAssertions;
using Xunit;

namespace AppCore.DependencyInjection
{
    public abstract class ComponentRegistryTests
    {
        public abstract IComponentRegistry Registry { get; }

        protected abstract IContainer BuildContainer();

        [Theory]
        [InlineData(ComponentLifetime.Transient)]
        [InlineData(ComponentLifetime.Scoped)]
        [InlineData(ComponentLifetime.Singleton)]
        public void RegisterWithFactory(ComponentLifetime lifetime)
        {
            var registration = ComponentRegistration.Create(
                typeof(IMyService),
                sp => new MyService(),
                lifetime);

            Registry.Register(registration);

            var services = BuildContainer()
                .ResolveAll<IMyService>();

            services.Should()
                    .HaveCount(1);

            services.Should()
                    .AllBeOfType<MyService>();
        }

        [Theory]
        [InlineData(ComponentLifetime.Transient)]
        [InlineData(ComponentLifetime.Scoped)]
        [InlineData(ComponentLifetime.Singleton)]
        public void RegisterOnlyOnceWithFactory(ComponentLifetime lifetime)
        {
            var registration = ComponentRegistration.Create(
                typeof(IMyService),
                sp => new MyService(),
                lifetime);

            Registry.Register(registration);

            var registration2 = ComponentRegistration.Create(
                typeof(IMyService),
                sp => new MyService2(),
                lifetime,
                ComponentRegistrationFlags.SkipIfRegistered);

            Registry.Register(registration2);

            var services = BuildContainer()
                .ResolveAll<IMyService>();

            services.Should()
                    .HaveCount(1);

            services.Should()
                    .AllBeOfType<MyService>();
        }

        [Theory]
        [InlineData(ComponentLifetime.Transient)]
        [InlineData(ComponentLifetime.Scoped)]
        [InlineData(ComponentLifetime.Singleton)]
        public void RegisterOnlyOnceEnumerableWithFactory(ComponentLifetime lifetime)
        {
            var registration = ComponentRegistration.Create(
                typeof(IMyService),
                sp => new MyService(),
                lifetime);

            Registry.Register(registration);

            var registration2 = ComponentRegistration.Create(
                typeof(IMyService),
                sp => new MyService(),
                lifetime,
                ComponentRegistrationFlags.SkipIfRegistered | ComponentRegistrationFlags.Enumerable);

            Registry.Register(registration2);

            var services = BuildContainer()
                .ResolveAll<IMyService>();

            services.Should()
                    .HaveCount(1);

            services.Should()
                    .AllBeOfType<MyService>();
        }

        [Theory]
        [InlineData(ComponentLifetime.Transient)]
        [InlineData(ComponentLifetime.Scoped)]
        [InlineData(ComponentLifetime.Singleton)]
        public void RegisterMultipleEnumerableWithFactory(ComponentLifetime lifetime)
        {
            var registration = ComponentRegistration.Create(
                typeof(IMyService),
                sp => new MyService(),
                lifetime);

            Registry.Register(registration);

            var registration2 = ComponentRegistration.Create(
                typeof(IMyService),
                sp => new MyService2(),
                lifetime,
                ComponentRegistrationFlags.SkipIfRegistered | ComponentRegistrationFlags.Enumerable);

            Registry.Register(registration2);

            var services = BuildContainer()
                .ResolveAll<IMyService>();

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
        [InlineData(ComponentLifetime.Transient)]
        [InlineData(ComponentLifetime.Scoped)]
        [InlineData(ComponentLifetime.Singleton)]
        public void RegisterWithType(ComponentLifetime lifetime)
        {
            var registration = ComponentRegistration.Create(
                typeof(IMyService),
                typeof(MyService),
                lifetime);

            Registry.Register(registration);

            var services = BuildContainer()
                .ResolveAll<IMyService>();

            services.Should()
                    .HaveCount(1);

            services.Should()
                    .AllBeOfType<MyService>();
        }

        [Theory]
        [InlineData(ComponentLifetime.Transient)]
        [InlineData(ComponentLifetime.Scoped)]
        [InlineData(ComponentLifetime.Singleton)]
        public void RegisterOnlyOnceWithType(ComponentLifetime lifetime)
        {
            var registration = ComponentRegistration.Create(
                typeof(IMyService),
                typeof(MyService),
                lifetime);

            Registry.Register(registration);

            var registration2 = ComponentRegistration.Create(
                typeof(IMyService),
                typeof(MyService2),
                lifetime,
                ComponentRegistrationFlags.SkipIfRegistered);

            Registry.Register(registration2);

            var services = BuildContainer()
                .ResolveAll<IMyService>();

            services.Should()
                    .HaveCount(1);

            services.Should()
                    .AllBeOfType<MyService>();
        }

        [Theory]
        [InlineData(ComponentLifetime.Transient)]
        [InlineData(ComponentLifetime.Scoped)]
        [InlineData(ComponentLifetime.Singleton)]
        public void RegisterOnlyOnceWithOpenGenericType(ComponentLifetime lifetime)
        {
            var registration = ComponentRegistration.Create(
                typeof(IGenericService<>),
                typeof(GenericService1<>),
                lifetime);

            Registry.Register(registration);

            var registration2 = ComponentRegistration.Create(
                typeof(IGenericService<>),
                typeof(GenericService1<>),
                lifetime,
                ComponentRegistrationFlags.SkipIfRegistered);

            Registry.Register(registration2);

            var services = BuildContainer()
                .ResolveAll<IGenericService<string>>();

            services.Should()
                    .HaveCount(1);

            services.Should()
                    .AllBeOfType<GenericService1<string>>();
        }

        [Theory]
        [InlineData(ComponentLifetime.Transient)]
        [InlineData(ComponentLifetime.Scoped)]
        [InlineData(ComponentLifetime.Singleton)]
        public void RegisterOnlyOnceEnumerableWithType(ComponentLifetime lifetime)
        {
            var registration = ComponentRegistration.Create(
                typeof(IMyService),
                typeof(MyService),
                lifetime);

            Registry.Register(registration);

            var registration2 = ComponentRegistration.Create(
                typeof(IMyService),
                typeof(MyService),
                lifetime,
                ComponentRegistrationFlags.SkipIfRegistered | ComponentRegistrationFlags.Enumerable);

            Registry.Register(registration2);

            var services = BuildContainer()
                .ResolveAll<IMyService>();

            services.Should()
                    .HaveCount(1);

            services.Should()
                    .AllBeOfType<MyService>();
        }

        [Theory]
        [InlineData(ComponentLifetime.Transient)]
        [InlineData(ComponentLifetime.Scoped)]
        [InlineData(ComponentLifetime.Singleton)]
        public void RegisterOnlyOnceEnumerableWithOpenGenericType(ComponentLifetime lifetime)
        {
            var registration = ComponentRegistration.Create(
                typeof(IGenericService<>),
                typeof(GenericService1<>),
                lifetime);

            Registry.Register(registration);

            var registration2 = ComponentRegistration.Create(
                typeof(IGenericService<>),
                typeof(GenericService1<>),
                lifetime,
                ComponentRegistrationFlags.SkipIfRegistered | ComponentRegistrationFlags.Enumerable);

            Registry.Register(registration2);

            var services = BuildContainer()
                .ResolveAll<IGenericService<string>>();

            services.Should()
                    .HaveCount(1);

            services.Should()
                    .AllBeOfType<GenericService1<string>>();
        }

        [Theory]
        [InlineData(ComponentLifetime.Transient)]
        [InlineData(ComponentLifetime.Scoped)]
        [InlineData(ComponentLifetime.Singleton)]
        public void RegisterMultipleEnumerableWithType(ComponentLifetime lifetime)
        {
            var registration = ComponentRegistration.Create(
                typeof(IMyService),
                typeof(MyService),
                lifetime);

            Registry.Register(registration);

            var registration2 = ComponentRegistration.Create(
                typeof(IMyService),
                typeof(MyService2),
                lifetime,
                ComponentRegistrationFlags.SkipIfRegistered | ComponentRegistrationFlags.Enumerable);

            Registry.Register(registration2);

            var services = BuildContainer()
                .ResolveAll<IMyService>();

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
            var registration = ComponentRegistration.Create(
                typeof(IMyService),
                new MyService());

            Registry.Register(registration);

            var services = BuildContainer()
                .ResolveAll<IMyService>();

            services.Should()
                    .HaveCount(1);

            services.Should()
                    .AllBeOfType<MyService>();
        }

        [Fact]
        public void RegisterOnlyOnceWithInstance()
        {
            var registration = ComponentRegistration.Create(
                typeof(IMyService),
                new MyService());

            Registry.Register(registration);

            var registration2 = ComponentRegistration.Create(
                typeof(IMyService),
                new MyService2(),
                ComponentRegistrationFlags.SkipIfRegistered);

            Registry.Register(registration2);

            var services = BuildContainer()
                .ResolveAll<IMyService>();

            services.Should()
                    .HaveCount(1);

            services.Should()
                    .AllBeOfType<MyService>();
        }

        [Fact]
        public void RegisterOnlyOnceEnumerableWithInstance()
        {
            var registration = ComponentRegistration.Create(
                typeof(IMyService),
                new MyService());

            Registry.Register(registration);

            var registration2 = ComponentRegistration.Create(
                typeof(IMyService),
                new MyService(),
                ComponentRegistrationFlags.SkipIfRegistered | ComponentRegistrationFlags.Enumerable);

            Registry.Register(registration2);

            var services = BuildContainer()
                .ResolveAll<IMyService>();

            services.Should()
                    .HaveCount(1);

            services.Should()
                    .AllBeOfType<MyService>();
        }

        [Fact]
        public void RegisterMultipleEnumerableWithInstance()
        {
            var registration = ComponentRegistration.Create(
                typeof(IMyService),
                new MyService());

            Registry.Register(registration);

            var registration2 = ComponentRegistration.Create(
                typeof(IMyService),
                new MyService2(),
                ComponentRegistrationFlags.SkipIfRegistered | ComponentRegistrationFlags.Enumerable);

            Registry.Register(registration2);

            var services = BuildContainer()
                .ResolveAll<IMyService>();

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
        [InlineData(ComponentLifetime.Transient)]
        [InlineData(ComponentLifetime.Scoped)]
        [InlineData(ComponentLifetime.Singleton)]
        public void RegisterAssembly(ComponentLifetime lifetime)
        {
            var registration = ComponentAssemblyRegistration.Create(
                new[]
                {
                    typeof(ComponentRegistryTests).Assembly
                },
                typeof(IMyService),
                lifetime);

            Registry.RegisterAssembly(registration);

            var services = BuildContainer()
                .ResolveAll<IMyService>();

            services.Should()
                    .HaveCount(2);

            services.Select(s => s.GetType())
                    .ShouldBeEquivalentTo(
                        new[]
                        {
                            typeof(MyService),
                            typeof(MyService2)
                        });
        }

        [Theory]
        [InlineData(ComponentLifetime.Transient)]
        [InlineData(ComponentLifetime.Scoped)]
        [InlineData(ComponentLifetime.Singleton)]
        public void RegisterAssemblyGenericServices(ComponentLifetime lifetime)
        {
            var registration = ComponentAssemblyRegistration.Create(
                new[]
                {
                    typeof(ComponentRegistryTests).Assembly
                },
                typeof(IGenericService<>),
                lifetime);

            Registry.RegisterAssembly(registration);

            var services = BuildContainer()
                .ResolveAll<IGenericService<string>>();

            services.Should()
                    .HaveCount(3);

            services.Select(s => s.GetType())
                    .ShouldBeEquivalentTo(
                        new[]
                        {
                            typeof(GenericService1<string>),
                            typeof(GenericService2<string>),
                            typeof(ClosedGenericService)
                        });
        }

        [Theory]
        [InlineData(ComponentLifetime.Transient)]
        [InlineData(ComponentLifetime.Scoped)]
        [InlineData(ComponentLifetime.Singleton)]
        public void RegisterAssemblyOnlyOnce(ComponentLifetime lifetime)
        {
            var registration = ComponentAssemblyRegistration.Create(
                new[]
                {
                    typeof(ComponentRegistryTests).Assembly
                },
                typeof(IMyService),
                lifetime,
                ComponentRegistrationFlags.SkipIfRegistered);

            Registry.RegisterAssembly(registration);

            var services = BuildContainer()
                .ResolveAll<IMyService>();

            services.Should()
                    .HaveCount(1);
        }

        [Theory]
        [InlineData(ComponentLifetime.Transient)]
        [InlineData(ComponentLifetime.Scoped)]
        [InlineData(ComponentLifetime.Singleton)]
        public void RegisterAssemblyOnlyOnceEnumerable(ComponentLifetime lifetime)
        {
            var registration = ComponentAssemblyRegistration.Create(
                new[]
                {
                    typeof(ComponentRegistryTests).Assembly
                },
                typeof(IMyService),
                lifetime,
                ComponentRegistrationFlags.SkipIfRegistered | ComponentRegistrationFlags.Enumerable);

            Registry.RegisterAssembly(registration);
            Registry.RegisterAssembly(registration);

            var services = BuildContainer()
                .ResolveAll<IMyService>();

            services.Should()
                    .HaveCount(2);
        }
    }
}