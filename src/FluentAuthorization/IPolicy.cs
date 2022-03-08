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

    public interface IPolicy<TUser> : IPolicy
    {

    }

    public interface IPolicyWithResource<TUser, in T> : IPolicy
    {

    }

    public interface IPolicyWithData<in T> : IPolicy
    {

    }

    public interface IPolicyWithData<TUser, in T> : IPolicyWithData<T>, IPolicy<TUser>
    {

    }

    public interface IPolicy<TUser, TResource, TData> : IPolicyWithData<TUser, TData>, IPolicyWithResource<TUser, TResource>
    {

    }
}
