// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System.Collections.Generic;

namespace AppCore.DependencyInjection
{
    public interface IGenericService<T>
    {
    }

    public interface IGenericService<T1, T2>
        where T2: IEnumerable<T1>
    {
    }
}
