﻿using System;
using System.Collections.Concurrent;

namespace FluentAuthorization
{
    internal class PolicyContextProvider<TUser, TResource> : IPolicyContextProvider<TUser, TResource>
    {
        private readonly TResource resource;
        private readonly IUserContextProvider<TUser> userContextProvider;
        private readonly IPolicyDataProvider<TUser> dataProvider;

        public PolicyContextProvider(
            TResource resource,
            IUserContextProvider<TUser> userContextProvider,
            IPolicyDataProvider<TUser> dataProvider)
        {
            this.resource = resource ?? throw new ArgumentNullException(nameof(resource));
            this.userContextProvider = userContextProvider ?? throw new ArgumentNullException(nameof(userContextProvider));
            this.dataProvider = dataProvider ?? throw new ArgumentNullException(nameof(dataProvider));
        }

        public IPolicyContextProvider<TUser, T, TResource> ForPolicy<T>() where T : class, IPolicyWithResource<TUser, TResource>, new()
        {
            var policy = PolicyProvider.Get<T>();

            return PolicyContextProviderFactory<TUser, T, TResource>.Build(policy, resource, userContextProvider, dataProvider);
        }
    }
}
