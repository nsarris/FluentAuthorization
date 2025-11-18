using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public AssertionResult Assert(string permissionName)
        {
            var permission = PolicyReflector.GetPermission<T, TUser, TResource, TData>(Policy, permissionName);
            var typedPermission = (Policy<TUser, TResource, TData>.Permission)permission;
            return Policy.Assert(user, Resource, typedPermission, Data);
        }

        public AssertionResult Assert<TState>(Func<T, IPermission<TState>> select, TState state)
        {
            var permission = select(Policy);
            var typedPermission = (Policy<TUser, TResource, TData>.Permission<TState>)permission;
            return Policy.Assert(user, Resource, typedPermission, Data, state);
        }

        public AssertionResult Assert<TState>(string permissionName, TState state)
        {
            var permission = PolicyReflector.GetPermission<T, TUser, TResource, TData, TState>(Policy, permissionName);
            var typedPermission = (Policy<TUser, TResource, TData>.Permission<TState>)permission;
            return Policy.Assert(user, Resource, typedPermission, Data, state);
        }

        public async Task<AssertionResult> AssertAsync(Func<T, IAsyncPermission> select)
        {
            var permission = select(Policy);
            var typedPermission = (Policy<TUser, TResource, TData>.AsyncPermission)permission;
            return await Policy.AssertAsync(user, Resource, typedPermission, Data);
        }

        public async Task<AssertionResult> AssertAsync(string permissionName)
        {
            var permission = PolicyReflector.GetAsyncPermission<T, TUser, TResource, TData>(Policy, permissionName);
            var typedPermission = (Policy<TUser, TResource, TData>.AsyncPermission)permission;
            return await Policy.AssertAsync(user, Resource, typedPermission, Data);
        }

        public async Task<AssertionResult> AssertAsync<TState>(Func<T, IAsyncPermission<TState>> select, TState state)
        {
            var permission = select(Policy);
            var typedPermission = (Policy<TUser, TResource, TData>.AsyncPermission<TState>)permission;
            return await Policy.AssertAsync(user, Resource, typedPermission, Data, state);
        }

        public async Task<AssertionResult> AssertAsync<TState>(string permissionName, TState state)
        {
            var permission = PolicyReflector.GetAsyncPermission<T, TUser, TResource, TData, TState>(Policy, permissionName);
            var typedPermission = (Policy<TUser, TResource, TData>.AsyncPermission<TState>)permission;
            return await Policy.AssertAsync(user, Resource, typedPermission, Data, state);
        }
    }
}
