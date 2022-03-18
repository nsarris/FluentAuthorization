using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentAuthorization
{
    internal class PolicyContext<T, TUser, TResource, TData> : IPolicyContext<T>, IDataContainer<TData>
        where T : Policy<TUser, TResource, TData>
    {
        private readonly TUser user;
        
        public PolicyContext(
            T policy,
            TUser user,
            TResource resource,
            IEnumerable<TData> data)
        {
            Data = data;
            Resource = resource;
            Policy = policy;
            this.user = user;
        }

        public IEnumerable<TData> Data { get; }
        public T Policy { get; }
        public TResource Resource { get; }

        public AssertionResult Assert(Func<T, IPermission> select)
        {
            var permission = select(Policy);
            var typedPermission = (Policy<TUser, TResource, TData>.Permission)permission;
            return Policy.Assert(user, Resource, typedPermission, Data);
        }

        public AssertionResult Assert<TState>(Func<T, IPermission<TState>> select, TState state)
        {
            var permission = select(Policy);
            var typedPermission = (Policy<TUser, TResource, TData>.Permission<TState>)permission;
            return Policy.Assert(user, Resource, typedPermission, Data, state);
        }
    }
}
