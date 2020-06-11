// Licensed under the MIT License.
// Copyright (c) 2020 the AppCore .NET project.

using System.Threading.Tasks;

namespace AppCore.Hosting
{
    /// <summary>
    /// Provides application lifetime events.
    /// </summary>
    public interface IApplicationLifetime
    {
        /// <summary>
        /// Signaled when the application has started up.
        /// </summary>
        Task Started { get; }

        /// <summary>
        /// Signaled when the application is stopping.
        /// </summary>
        Task Stopping { get; }

        /// <summary>
        /// Signaled when the application has stopped.
        /// </summary>
        Task Stopped { get; }

        /// <summary>
        /// Requests termination of the application.
        /// </summary>
        void Stop();
    }
}
