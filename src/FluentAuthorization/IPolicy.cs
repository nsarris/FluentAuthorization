using System;

namespace FluentAuthorization
{
    public interface IPolicy
    {
        string Key { get; }
        string Name { get; }
        Type DataType { get; }
        Type UserType { get; }
        Type ResourceType { get; }
    }

    public interface IPolicyWithResource<in T> : IPolicy
    {

    }

    public interface IPolicyWithData<in T> : IPolicy
    {

    }

    public interface IPolicy<TResource, TData> : IPolicyWithData<TData>, IPolicyWithResource<TResource>
    {

    }
}
