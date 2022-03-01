using System;

namespace FluentAuthorization
{
    internal class PolicyContextBuilder<TUser, TResource> : IPolicyContextBuilder<TResource>
    {
        private readonly TResource resource;
        private readonly IUserContextProvider<TUser> userContextProvider;
        private readonly IPolicyProvider policyProvider;
        private readonly IPolicyDataProvider<TUser> dataProvider;
        private readonly IServiceProvider serviceProvider;

        public PolicyContextBuilder(
            TResource resource,
            IUserContextProvider<TUser> userContextProvider,
            IPolicyProvider policyProvider,
            IPolicyDataProvider<TUser> dataProvider,
            IServiceProvider serviceProvider = null)
        {
            this.resource = resource ?? throw new ArgumentNullException(nameof(resource));
            this.userContextProvider = userContextProvider ?? throw new ArgumentNullException(nameof(userContextProvider));
            this.policyProvider = policyProvider ?? throw new ArgumentNullException(nameof(policyProvider));
            this.dataProvider = dataProvider ?? throw new ArgumentNullException(nameof(dataProvider));
            this.serviceProvider = serviceProvider;
        }

        public IPolicyContextBuilder<T, TResource> ForPolicy<T>() where T : class, IPolicyWithResource<TResource>
        {
            var policy = policyProvider.Get<T>();

            var providerType = typeof(PolicyContextBuilder<,,,>).MakeGenericType(typeof(T), typeof(TUser), policy.ResourceType, policy.DataType);
            return (IPolicyContextBuilder<T, TResource>)Activator.CreateInstance(providerType, new object[] { policy, resource, userContextProvider, dataProvider, serviceProvider });
        }
    }
}
