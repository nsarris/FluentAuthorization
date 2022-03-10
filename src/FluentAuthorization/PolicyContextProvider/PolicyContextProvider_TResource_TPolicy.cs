using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FluentAuthorization
{
    internal class PolicyContextProvider<T, TUser, TResource, TData> :
            IPolicyContextProvider<TUser, T, TResource, TData>,
            IDataProvider<TData>
        where T : Policy<TUser, TResource, TData>
    {
        private readonly AsyncLazy<TUser> user;
        private readonly AsyncLazy<IEnumerable<TData>> data;

        public T Policy { get; }
        public TResource Resource { get; }

        public PolicyContextProvider(
            T policy,
            TResource resource,
            IUserContextProvider<TUser> userContextProvider,
            IPolicyDataProvider<TUser> dataProvider)
        {
            Policy = policy;
            Resource = resource;
            
            user = new AsyncLazy<TUser>(() => userContextProvider.GetAsync());
            data = new AsyncLazy<IEnumerable<TData>>(async () => await dataProvider.GetDataAsync<T, TResource, TData>(await user.Value.ConfigureAwait(false), Policy, Resource));
        }

        async Task<IPolicyContext<T>> IPolicyContextProvider<TUser, T, TResource>.BuildContextAsync()
        {
            var contextUser = await user.Value.ConfigureAwait(false);
            var contextData = await data.Value.ConfigureAwait(false);
            
            return new PolicyContext<T, TUser, TResource, TData>(Policy, contextUser, Resource, contextData);
        }

        async Task<IPolicyContext<T>> IPolicyContextProvider<TUser, T, TResource>.BuildContextAsync(TUser user)
        {
            var contextData = await data.Value.ConfigureAwait(false);

            return new PolicyContext<T, TUser, TResource, TData>(Policy, user, Resource, contextData);
        }

        async Task<IPolicyContext<T>> IPolicyContextProvider<TUser, T, TResource, TData>.BuildContextAsync(TData data)
        {
            var contextUser = await user.Value.ConfigureAwait(false);
            
            return new PolicyContext<T, TUser, TResource, TData>(Policy, contextUser, Resource, new[] { data });
        }

        IPolicyContext<T> IPolicyContextProvider<TUser, T, TResource, TData>.BuildContext(TUser user, TData data)
        {
            return new PolicyContext<T, TUser, TResource, TData>(Policy, user, Resource, new[] { data });
        }

        Task<IEnumerable<TData>> IDataProvider<TData>.GetDataAsync()
            => data.Value;
    }
}
