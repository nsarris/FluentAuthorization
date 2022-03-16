using System;
using System.Collections.Generic;

namespace FluentAuthorization
{
    /// <summary>
    /// Abtraction of a policy. Internal marker interface, not intended for implementation.
    /// </summary>
    public interface IPolicy
    {
        string Key { get; }
        string Name { get; }
        Type UserType { get; }
        Type DataType { get; }
        Type ResourceType { get; }
    }

    /// <summary>
    /// Abtraction of a policy with a user and resource. Internal marker interface, not intended for implementation.
    /// </summary>
    public interface IPolicyWithResource<TUser, TResource> : IPolicy
    {

    }

    /// <summary>
    /// Abtraction of a policy with data. Internal marker interface, not intended for implementation.
    /// </summary>
    public interface IPolicyWithData<T> : IPolicy
    {
        T Aggregate(IEnumerable<T> data);
    }

    /// <summary>
    /// Abtraction of a policy with user, resource and data. Internal marker interface, not intended for implementation.
    /// </summary>
    public interface IPolicy<TUser, TResource, TData> : IPolicyWithData<TData>, IPolicyWithResource<TUser, TResource>
    {

    }
}
