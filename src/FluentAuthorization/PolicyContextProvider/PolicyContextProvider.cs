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

        class CachedUserProvider : IUserContextProvider<TUser>
        {
            private readonly AsyncLazy<TUser> user;

            public CachedUserProvider(IUserContextProvider<TUser> provider)
            {
                this.user = new AsyncLazy<TUser>(provider.GetAsync);
            }

            public Task<TUser> GetAsync()
            {
                return user.Value;
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

        public IPolicyContextProvider<TUser> EnableUserCaching()
        { 
            return new PolicyContextProvider<TUser>(
                new CachedUserProvider(userContextProvider),
                dataProvider
                );
        }

        public IPolicyContextProvider<TUser, TPolicy> ForPolicy<TPolicy>() where TPolicy : class, IPolicy<TUser>, new()
        {
            return PolicyContextProviderFactory<TUser, TPolicy>.Build(
                PolicyProvider.Get<TPolicy>(),
                userContextProvider,
                dataProvider);
        }
    }
}
