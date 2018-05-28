// Licensed under the MIT License.
// Copyright (c) 2018 the AppCore .NET project.

namespace AppCore.DependencyInjection
{
    public abstract class ContractImplBase : IContract
    {
    }

    public abstract class ContractImplBase<T> : IContract<T>
    {
    }

    public abstract class ContractImplBaseString : ContractImplBase<string>
    {
    }
}