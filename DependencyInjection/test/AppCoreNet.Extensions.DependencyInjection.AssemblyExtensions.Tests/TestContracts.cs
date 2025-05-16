// Licensed under the MIT license.
// Copyright (c) The AppCore .NET project.

namespace AppCoreNet.Extensions.DependencyInjection;

public static class TestContracts
{
    public interface IContract
    {
    }

    public interface IContract<T>
    {
    }

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

    public class ContractImpl2 : ContractImplBase
    {
    }

    public class ContractImpl2<T> : ContractImplBase<T>
    {
    }

    public class ContractImpl2String : ContractImplBaseString
    {
    }

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