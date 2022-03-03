//using Microsoft.Extensions.DependencyInjection;
//using System;

//namespace FluentAuthorization.DependencyInjection
//{
//    class DefaultPolicyProvider : IPolicyProvider
//    {
//        private readonly IServiceProvider serviceProvider;

//        public DefaultPolicyProvider(IServiceProvider serviceProvider)
//        {
//            this.serviceProvider = serviceProvider;
//        }

//        public T Get<T>() where T : class, IPolicy, new()
//            => serviceProvider.GetService<T>();
//    }
//}

