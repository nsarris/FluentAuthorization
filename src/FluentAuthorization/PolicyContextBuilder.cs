using System;

namespace FluentAuthorization
{
    internal class PolicyContextBuilder<TUser, TResource> : IPolicyContextBuilder<TUser, TResource>
    {
        private readonly TResource resource;
        private readonly IUserContextProvider<TUser> userContextProvider;
        private readonly IPolicyDataProvider<TUser> dataProvider;

        public PolicyContextBuilder(
            TResource resource,
            IUserContextProvider<TUser> userContextProvider,
            IPolicyDataProvider<TUser> dataProvider)
        {
            this.resource = resource ?? throw new ArgumentNullException(nameof(resource));
            this.userContextProvider = userContextProvider ?? throw new ArgumentNullException(nameof(userContextProvider));
            this.dataProvider = dataProvider ?? throw new ArgumentNullException(nameof(dataProvider));
        }

        public IPolicyContextBuilder<TUser, T, TResource> ForPolicy<T>() where T : class, IPolicyWithResource<TUser, TResource>, new()
        {
            var policy = PolicyProvider.Get<T>();

            var providerType = typeof(PolicyContextBuilder<,,,>).MakeGenericType(typeof(T), typeof(TUser), policy.ResourceType, policy.DataType);
            return (IPolicyContextBuilder<TUser, T, TResource>)Activator.CreateInstance(providerType, new object[] { policy, resource, userContextProvider, dataProvider });
        }
    }
}
