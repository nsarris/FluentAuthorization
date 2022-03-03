using System;

namespace FluentAuthorization
{
    internal class PolicyContext<T, TUser, TResource, TData> : IPolicyContext<T>, IPolicyContextDataInternal<TData>
        where T : Policy<TUser, TResource, TData>
    {
        private readonly TUser user;
        
        public PolicyContext(
            T policy,
            TUser user,
            TData data)
        {
            Data = data;
            Policy = policy;
            this.user = user;
        }

        public TData Data { get; }
        public T Policy { get; }

        //TData IPolicyContextDataInternal<TData>.Data => Data;

        //public AssertionResult Assert(Func<T, Policy<TUser, TResource, TData>.Permission> select)
        //{
        //    var permission = select(Policy);

        //    return permission.Assert(new Policy<TUser, TResource, TData>.AssertionContext(user, Data, serviceProvider));
        //}

        //public AssertionResult Assert<TState>(Func<T, Policy<TUser, TResource, TData>.Permission<TState>> select, TState state)
        //{
        //    var permission = select(Policy);
        //    return permission.Assert(new Policy<TUser, TResource, TData>.AssertionContext<TState>(user, Data, state, serviceProvider));
        //}

        public AssertionResult Assert(Func<T, IPermission> select)
        {
            var permission = select(Policy);

            var typedPermission = (Policy<TUser, TResource, TData>.Permission)permission;

            return typedPermission.Assert(new Policy<TUser, TResource, TData>.AssertionContext(user, Data));
        }

        public AssertionResult Assert<TState>(Func<T, IPermission<TState>> select, TState state)
        {
            var permission = select(Policy);

            var typedPermission = (Policy<TUser, TResource, TData>.Permission<TState>)permission;

            return typedPermission.Assert(new Policy<TUser, TResource, TData>.AssertionContext<TState>(user, Data, state));
        }
    }
}
