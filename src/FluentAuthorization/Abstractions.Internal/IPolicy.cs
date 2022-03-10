using System;
using System.Collections.Generic;

namespace FluentAuthorization
{
    public interface IPolicy
    {
        string Key { get; }
        string Name { get; }
        Type UserType { get; }
        Type DataType { get; }
        Type ResourceType { get; }
    }

    public interface IPolicyWithResource<TUser, TResource> : IPolicy
    {

    }

    public interface IPolicyWithData<T> : IPolicy
    {
        T Aggregate(IEnumerable<T> data);
    }

    public interface IPolicy<TUser, TResource, TData> : IPolicyWithData<TData>, IPolicyWithResource<TUser, TResource>
    {

    }
}
