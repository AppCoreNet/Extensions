// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System.Collections.Generic;
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
                Factory.Create(sp => new MyService()),
                lifetime);

            Registry.Add(registration);

            IEnumerable<IMyService> services = BuildContainer()
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
                Factory.Create(sp => new MyService()),
                lifetime);

            Registry.Add(registration);

            var registration2 = ComponentRegistration.Create(
                typeof(IMyService),
                Factory.Create(sp => new MyService2()),
                lifetime);

            Registry.TryAdd(registration2);

            IEnumerable<IMyService> services = BuildContainer()
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
                Factory.Create(sp => new MyService()),
                lifetime);

            Registry.Add(registration);

            var registration2 = ComponentRegistration.Create(
                typeof(IMyService),
                Factory.Create(sp => new MyService()),
                lifetime);

            Registry.TryAddEnumerable(registration2);

            IEnumerable<IMyService> services = BuildContainer()
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
            var registration = ComponentRegistration.Create<IMyService>(
                Factory.Create(sp => new MyService()),
                lifetime);

            Registry.Add(registration);

            var registration2 = ComponentRegistration.Create(
                typeof(IMyService),
                Factory.Create(sp => new MyService2()),
                lifetime);

            Registry.TryAddEnumerable(registration2);

            IEnumerable<IMyService> services = BuildContainer()
                .ResolveAll<IMyService>();

            services.Should()
                    .HaveCount(2);

            services.Select(s => s.GetType())
                    .Should()
                    .BeEquivalentTo(
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

            Registry.Add(registration);

            IEnumerable<IMyService> services = BuildContainer()
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

            Registry.Add(registration);

            var registration2 = ComponentRegistration.Create(
                typeof(IMyService),
                typeof(MyService2),
                lifetime);

            Registry.TryAdd(registration2);

            IEnumerable<IMyService> services = BuildContainer()
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

            Registry.Add(registration);

            var registration2 = ComponentRegistration.Create(
                typeof(IGenericService<>),
                typeof(GenericService1<>),
                lifetime);

            Registry.TryAddEnumerable(registration2);

            IEnumerable<IGenericService<string>> services = BuildContainer()
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
        public void RegisterOnlyOnceWithOpenGenericType2(ComponentLifetime lifetime)
        {
            var registration = ComponentRegistration.Create(
                typeof(IGenericService<,>),
                typeof(GenericService1<,>),
                lifetime);

            Registry.Add(registration);

            var registration2 = ComponentRegistration.Create(
                typeof(IGenericService<,>),
                typeof(GenericService1<,>),
                lifetime);

            Registry.TryAddEnumerable(registration2);

            IEnumerable<IGenericService<string,IEnumerable<string>>> services = BuildContainer()
                .ResolveAll<IGenericService<string,IEnumerable<string>>>();

            services.Should()
                    .HaveCount(1);

            services.Should()
                    .AllBeOfType<GenericService1<string,IEnumerable<string>>>();
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

            Registry.Add(registration);

            var registration2 = ComponentRegistration.Create(
                typeof(IMyService),
                typeof(MyService),
                lifetime);

            Registry.TryAdd(registration2);

            IEnumerable<IMyService> services = BuildContainer()
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

            Registry.Add(registration);

            var registration2 = ComponentRegistration.Create(
                typeof(IGenericService<>),
                typeof(GenericService1<>),
                lifetime);

            Registry.TryAddEnumerable(registration2);

            IEnumerable<IGenericService<string>> services = BuildContainer()
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
        public void RegisterOnlyOnceEnumerableWithOpenGenericType2(ComponentLifetime lifetime)
        {
            var registration = ComponentRegistration.Create(
                typeof(IGenericService<,>),
                typeof(GenericService1<,>),
                lifetime);

            Registry.Add(registration);

            var registration2 = ComponentRegistration.Create(
                typeof(IGenericService<,>),
                typeof(GenericService1<,>),
                lifetime);

            Registry.TryAddEnumerable(registration2);

            IEnumerable<IGenericService<string,IEnumerable<string>>> services = BuildContainer()
                .ResolveAll<IGenericService<string,IEnumerable<string>>>();

            services.Should()
                    .HaveCount(1);

            services.Should()
                    .AllBeOfType<GenericService1<string, IEnumerable<string>>>();
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

            Registry.Add(registration);

            var registration2 = ComponentRegistration.Create(
                typeof(IMyService),
                typeof(MyService2),
                lifetime);

            Registry.TryAddEnumerable(registration2);

            IEnumerable<IMyService> services = BuildContainer()
                .ResolveAll<IMyService>();

            services.Should()
                    .HaveCount(2);

            services.Select(s => s.GetType())
                    .Should()
                    .BeEquivalentTo(
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

            Registry.Add(registration);

            IEnumerable<IMyService> services = BuildContainer()
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

            Registry.Add(registration);

            var registration2 = ComponentRegistration.Create(
                typeof(IMyService),
                new MyService2());

            Registry.TryAdd(registration2);

            IEnumerable<IMyService> services = BuildContainer()
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

            Registry.Add(registration);

            var registration2 = ComponentRegistration.Create(
                typeof(IMyService),
                new MyService());

            Registry.TryAddEnumerable(registration2);

            IEnumerable<IMyService> services = BuildContainer()
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

            Registry.Add(registration);

            var registration2 = ComponentRegistration.Create(
                typeof(IMyService),
                new MyService2());

            Registry.TryAddEnumerable(registration2);

            IEnumerable<IMyService> services = BuildContainer()
                .ResolveAll<IMyService>();

            services.Should()
                    .HaveCount(2);

            services.Select(s => s.GetType())
                    .Should()
                    .BeEquivalentTo(
                        new[]
                        {
                            typeof(MyService),
                            typeof(MyService2)
                        },
                        o => o.WithStrictOrdering());
        }

        /*
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
                ComponentRegistrationFlags.IfNoneRegistered);

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
                ComponentRegistrationFlags.IfNoneRegistered | ComponentRegistrationFlags.IfNotRegistered);

            Registry.RegisterAssembly(registration);
            Registry.RegisterAssembly(registration);

            var services = BuildContainer()
                .ResolveAll<IMyService>();

            services.Should()
                    .HaveCount(2);
        }
        */
    }
}