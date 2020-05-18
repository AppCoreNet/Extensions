// Licensed under the MIT License.
// Copyright (c) 2020 the AppCore .NET project.

using System.Threading;
using System.Threading.Tasks;

namespace AppCore.Hosting
{
    public interface IStartupTask
    {
        int Order { get; }

        Task ExecuteAsync(CancellationToken cancellationToken);
    }
}
