// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

using System.Collections.Generic;

namespace AppCore.DependencyInjection
{
    public class GenericService1<T> : IGenericService<T>
    {
    }

    public class GenericService1<T1,T2> : IGenericService<T1,T2>
        where T2 : IEnumerable<T1>
    {
    }
}