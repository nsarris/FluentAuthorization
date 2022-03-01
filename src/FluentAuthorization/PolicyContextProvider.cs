using System;
using System.Threading.Tasks;

namespace FluentAuthorization
{
    public class PolicyContextProvider<TUser> : IPolicyContextProvider<TUser>
    {
        private readonly IUserContextProvider<TUser> userContextProvider;
        private readonly IPolicyProvider policyProvider;
        private readonly IPolicyDataProvider<TUser> dataProvider;
        private readonly IServiceProvider serviceProvider;

        class StaticUserProvider : IUserContextProvider<TUser>
        {
            private readonly TUser user;

            public StaticUserProvider(TUser user)
            {
                this.user = user;
            }
            public Task<TUser> GetAsync()
            {
                return Task.FromResult(user);
            }
        }

        public PolicyContextProvider(
            IUserContextProvider<TUser> userContextProvider,
            IPolicyProvider policyProvider,
            IPolicyDataProvider<TUser> dataProvider,
            IServiceProvider serviceProvider = null)
        {
            this.userContextProvider = userContextProvider;
            this.policyProvider = policyProvider;
            this.dataProvider = dataProvider;
            this.serviceProvider = serviceProvider;
        }

        public IPolicyContextProvider<TUser> ForUser(TUser user)
        {
            if (user is null) throw new ArgumentNullException(nameof(user));

            return new PolicyContextProvider<TUser>(
                new StaticUserProvider(user),
                policyProvider,
                dataProvider,
                serviceProvider);
        }

        public IPolicyContextBuilder<TResource> ForResource<TResource>(TResource resource)
        {
            if (resource is null) throw new ArgumentNullException(nameof(resource));

            return new PolicyContextBuilder<TUser, TResource>(resource, userContextProvider, policyProvider, dataProvider, serviceProvider);
        }
    }
}
