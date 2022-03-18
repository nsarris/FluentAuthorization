using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FluentAuthorization
{
    internal class PolicyContextProvider<TPolicy, TUser, TResource, TData> :
            IPolicyContextProviderWithData<TUser, TPolicy, TData>,
            IPolicyContextProvider<TUser, TPolicy, TResource>,
            IDataProvider<TData>
        where TPolicy : Policy<TUser, TResource, TData>
    {
        private readonly IUserContextProvider<TUser> userContextProvider;
        private readonly IPolicyDataProvider<TUser> dataProvider;

        public TPolicy Policy { get; }
        public TResource Resource { get; private set;  }

        public PolicyContextProvider(
            TPolicy policy,
            IUserContextProvider<TUser> userContextProvider,
            IPolicyDataProvider<TUser> dataProvider)
        {
            Policy = policy;
            this.userContextProvider = userContextProvider;
            this.dataProvider = dataProvider;
        }

        public PolicyContextProvider(
            TPolicy policy,
            TResource resource,
            IUserContextProvider<TUser> userContextProvider,
            IPolicyDataProvider<TUser> dataProvider)
            :this(policy, userContextProvider, dataProvider)
        {
            Resource = resource;
        }

        private Task<IEnumerable<TData>> GetDataAsync(TUser user)
        {
            return dataProvider.GetDataAsync<TPolicy, TResource, TData>(user, Policy, Resource);
        }

        private async Task<IEnumerable<TData>> GetDataAsync()
        {
            return await GetDataAsync(
                await userContextProvider.GetAsync().ConfigureAwait(false))
                .ConfigureAwait(false);
        }

        async Task<IPolicyContext<TPolicy>> IPolicyContextProvider<TUser, TPolicy>.BuildContextAsync()
        {
            var contextUser = await userContextProvider.GetAsync().ConfigureAwait(false);
            var contextData = await GetDataAsync(contextUser).ConfigureAwait(false);
            
            return new PolicyContext<TPolicy, TUser, TResource, TData>(Policy, contextUser, Resource, contextData);
        }

        async Task<IPolicyContext<TPolicy>> IPolicyContextProvider<TUser, TPolicy>.BuildContextAsync(TUser user)
        {
            var contextData = await GetDataAsync(user);

            return new PolicyContext<TPolicy, TUser, TResource, TData>(Policy, user, Resource, contextData);
        }

        async Task<IPolicyContext<TPolicy>> IPolicyContextProviderWithData<TUser, TPolicy, TData>.BuildContextAsync(TData data)
        {
            var contextUser = await userContextProvider.GetAsync().ConfigureAwait(false);
                
            return new PolicyContext<TPolicy, TUser, TResource, TData>(Policy, contextUser, Resource, new[] { data });
        }

        IPolicyContext<TPolicy> IPolicyContextProviderWithData<TUser, TPolicy, TData>.BuildContext(TUser user, TData data)
        {
            return new PolicyContext<TPolicy, TUser, TResource, TData>(Policy, user, Resource, new[] { data });
        }

        async Task<IPolicyContext<TPolicy>> IPolicyContextProviderWithData<TUser, TPolicy, TData>.BuildContextAsync(IEnumerable<TData> data)
        {
            var contextUser = await userContextProvider.GetAsync().ConfigureAwait(false);

            return new PolicyContext<TPolicy, TUser, TResource, TData>(Policy, contextUser, Resource, data);
        }

        IPolicyContext<TPolicy> IPolicyContextProviderWithData<TUser, TPolicy, TData>.BuildContext(TUser user, IEnumerable<TData> data)
        {
            return new PolicyContext<TPolicy, TUser, TResource, TData>(Policy, user, Resource, data);
        }

        Task<IEnumerable<TData>> IDataProvider<TData>.GetDataAsync()
            => GetDataAsync();

        public IPolicyContextProvider<TUser, TPolicy, TResource> ForResource(TResource resource)
        {
            return new PolicyContextProvider<TPolicy, TUser, TResource, TData>(Policy, resource, userContextProvider, dataProvider);
        }
    }
}
