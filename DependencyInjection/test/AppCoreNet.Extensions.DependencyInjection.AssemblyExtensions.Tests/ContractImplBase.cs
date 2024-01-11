// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

namespace AppCoreNet.Extensions.DependencyInjection;

public abstract class ContractImplBase : IContract
{
}

public abstract class ContractImplBase<T> : IContract<T>
{
}

public abstract class ContractImplBaseString : ContractImplBase<string>
{
}