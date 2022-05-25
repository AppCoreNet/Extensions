// Licensed under the MIT License.
// Copyright (c) 2018,2019 the AppCore .NET project.

namespace AppCore.Extensions.DependencyInjection
{
    public class ContractImplOpenGeneric<T> : IContract
    {
    }

    public class ContractImpl1 : ContractImplBase
    {
    }

    public class ContractImpl1<T> : ContractImplBase<T>
    {
    }

    public class ContractImpl1String : ContractImplBaseString
    {
    }
}