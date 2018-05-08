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

            Registry.Register(
                ComponentRegistration.Create(
                    typeof(IDisposableService),
                    c => disposable,
                    ComponentLifetime.Scoped));

            using (IContainerScope scope = CreateScope())
            {
                scope.Container.Resolve<IDisposableService>();
            }

            disposable.Received(1)
                      .Dispose();
        }

        [Fact]
        public void DoesNotDisposeSingleton()
        {
            var disposable1 = Substitute.For<IDisposableService>();
            var disposable2 = Substitute.For<IDisposableService>();

            Registry.Register(
                ComponentRegistration.Create(
                    typeof(IDisposableService),
                    c => disposable1,
                    ComponentLifetime.Singleton));

            Registry.Register(
                ComponentRegistration.Create(
                    typeof(IDisposableService),
                    c => disposable2,
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
