using System;
using System.Threading.Tasks;

namespace FluentAuthorization
{
    public class PolicyContextProvider<TUser> : IPolicyContextProvider<TUser>
    {
        private readonly IUserContextProvider<TUser> userContextProvider;
        private readonly IPolicyDataProvider<TUser> dataProvider;
        
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
            IPolicyDataProvider<TUser> dataProvider)
        {
            this.userContextProvider = userContextProvider;
            this.dataProvider = dataProvider;
        }

        public IPolicyContextProvider<TUser> ForUser(TUser user)
        {
            if (user is null) throw new ArgumentNullException(nameof(user));

            return new PolicyContextProvider<TUser>(
                new StaticUserProvider(user),
                dataProvider
                );
        }

        public IPolicyContextBuilder<TUser, TResource> ForResource<TResource>(TResource resource)
        {
            if (resource is null) throw new ArgumentNullException(nameof(resource));

            return new PolicyContextBuilder<TUser, TResource>(
                resource, 
                userContextProvider, 
                dataProvider);
        }
    }
}
