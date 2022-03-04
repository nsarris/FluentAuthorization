using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FluentAuthorization
{
    internal class PolicyContextBuilder<T, TUser, TResource, TData> :
            IPolicyContextBuilder<T, TResource>,
            IPolicyContextProviderInternal<T, TResource>,
            IPolicyContextDataProviderInternal<TData>,
            IPolicyContextDataProviderInternal
        where T : Policy<TUser, TResource, TData>
    {
        private readonly AsyncLazy<TUser> user;
        private readonly AsyncLazy<IEnumerable<TData>> data;

        public T Policy { get; }
        public TResource Resource { get; }

        public PolicyContextBuilder(
            T policy,
            TResource resource,
            IUserContextProvider<TUser> userContextProvider,
            IPolicyDataProvider<TUser> dataProvider)
        {
            Policy = policy;
            Resource = resource;
            
            user = new AsyncLazy<TUser>(() => userContextProvider.GetAsync());
            data = new AsyncLazy<IEnumerable<TData>>(async () => await dataProvider.GetDataAsync<T, TResource, TData>(await GetUserAsync().ConfigureAwait(false), Policy, Resource));
        }

        async Task<IPolicyContext<T>> IPolicyContextProviderInternal<T, TResource>.BuildAsync()
        {
            var user = await GetUserAsync().ConfigureAwait(false);
            var data = await GetDataAsync().ConfigureAwait(false);
            
            return new PolicyContext<T, TUser, TResource, TData>(Policy, user, Resource, data);
        }

        private Task<TUser> GetUserAsync()
        {
            return user.Value;
        }

        private async Task<TData> GetDataAsync()
        {
            return (await data.Value.ConfigureAwait(false)).First();
        }

        Task<TData> IPolicyContextDataProviderInternal<TData>.GetDataAsync()
        {
            return GetDataAsync();
        }

        async Task<object> IPolicyContextDataProviderInternal.GetDataAsync()
        {
            return await GetDataAsync().ConfigureAwait(false);
        }
    }
}
