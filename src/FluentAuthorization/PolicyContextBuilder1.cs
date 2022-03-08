using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FluentAuthorization
{
    internal class PolicyContextBuilder<T, TUser, TResource, TData> :
            IPolicyContextBuilder<TUser, T, TResource>,
            IPolicyContextProviderInternal<TUser, T, TResource>,
            IPolicyContextProviderInternal<TUser, T, TResource, TData>,
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

        async Task<IPolicyContext<T>> IPolicyContextProviderInternal<TUser, T, TResource>.BuildAsync()
        {
            var contextUser = await GetUserAsync().ConfigureAwait(false);
            var contextData = await GetDataAsync().ConfigureAwait(false);
            
            return new PolicyContext<T, TUser, TResource, TData>(Policy, contextUser, Resource, contextData);
        }

        async Task<IPolicyContext<T>> IPolicyContextProviderInternal<TUser, T, TResource>.BuildAsync(TUser user)
        {
            var contextData = await GetDataAsync().ConfigureAwait(false);

            return new PolicyContext<T, TUser, TResource, TData>(Policy, user, Resource, contextData);
        }

        async Task<IPolicyContext<T>> IPolicyContextProviderInternal<TUser, T, TResource, TData>.BuildAsync(TData data)
        {
            var contextUser = await GetUserAsync().ConfigureAwait(false);
            
            return new PolicyContext<T, TUser, TResource, TData>(Policy, contextUser, Resource, data);
        }

        IPolicyContext<T> IPolicyContextProviderInternal<TUser, T, TResource, TData>.Build(TUser user, TData data)
        {
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
