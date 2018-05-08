namespace AppCore.DependencyInjection
{
    /// <summary>
    /// Represents a factory to create a <see cref="IContainerScope"/>.
    /// </summary>
    public interface IContainerScopeFactory
    {
        /// <summary>
        /// Creates a <see cref="IContainerScope"/>.
        /// </summary>
        /// <returns>The <see cref="IContainerScope"/>.</returns>
        IContainerScope CreateScope();
    }
}