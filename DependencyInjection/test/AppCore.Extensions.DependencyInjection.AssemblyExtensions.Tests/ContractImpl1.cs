// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

namespace AppCore.Extensions.DependencyInjection;

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