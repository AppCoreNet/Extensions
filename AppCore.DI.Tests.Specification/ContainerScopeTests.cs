// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using FluentAssertions;
using NSubstitute;
using Xunit;

namespace AppCore.DependencyInjection
{
    public abstract class ContainerScopeTests
    {
        public abstract IComponentRegistry Registry { get; }

        public abstract IContainerScope CreateScope();

        [Fact]
        public void DisposesScopedComponent()
        {
            var disposable = Substitute.For<IDisposableService>();

            Registry.Add(
                ComponentRegistration.Create(
                    typeof(IDisposableService),
                    ComponentFactory.Create(c => disposable),
                    ComponentLifetime.Scoped));

            using (IContainerScope scope = CreateScope())
            {
                scope.Container.Resolve<IDisposableService>();
            }

            disposable.Received(1)
                      .Dispose();
        }

        [Fact]
        public void ResolvesDifferentInstanceFromDifferentScope()
        {
            Registry.Add(
                ComponentRegistration.Create(typeof(IMyService), typeof(MyService), ComponentLifetime.Scoped));

            using (IContainerScope scope1 = CreateScope())
            using (IContainerScope scope2 = CreateScope())
            {
                var service1 = scope1.Container.Resolve<IMyService>();
                var service2 = scope2.Container.Resolve<IMyService>();

                service1.Should()
                        .NotBeSameAs(service2);
            }
        }

        [Fact]
        public void ResolvesSameInstanceFromSameScope()
        {
            Registry.Add(
                ComponentRegistration.Create(typeof(IMyService), typeof(MyService), ComponentLifetime.Scoped));

            using (IContainerScope scope = CreateScope())
            {
                var service1 = scope.Container.Resolve<IMyService>();
                var service2 = scope.Container.Resolve<IMyService>();

                service1.Should()
                        .BeSameAs(service2);
            }
        }

        [Fact]
        public void DoesNotDisposeSingleton()
        {
            var disposable1 = Substitute.For<IDisposableService>();
            var disposable2 = Substitute.For<IDisposableService>();

            Registry.Add(
                ComponentRegistration.Create(
                    typeof(IDisposableService),
                    ComponentFactory.Create(c => disposable1),
                    ComponentLifetime.Singleton));

            Registry.Add(
                ComponentRegistration.Create(
                    typeof(IDisposableService),
                    ComponentFactory.Create(c => disposable2),
                    ComponentLifetime.Scoped));

            using (IContainerScope scope = CreateScope())
            {
                scope.Container.ResolveAll<IDisposableService>();
            }

            disposable1.DidNotReceive()
                       .Dispose();

            disposable2.Received(1)
                       .Dispose();
        }
    }
}
