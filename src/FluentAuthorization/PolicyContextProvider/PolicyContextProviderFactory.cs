using System;
using System.Linq;
using System.Linq.Expressions;

namespace FluentAuthorization
{
    internal class PolicyContextProviderFactory<TUser, TPolicy, TResource>
        where TPolicy : class, IPolicyWithResource<TUser, TResource>, new()
    {
        public delegate IPolicyContextProvider<TUser, TPolicy, TResource> Constructor(
            TPolicy policy, 
            TResource resource, 
            IUserContextProvider<TUser> userContextProvider, 
            IPolicyDataProvider<TUser> policyDataProvider);

        private static Constructor constructor;

        public static IPolicyContextProvider<TUser, TPolicy, TResource> Build(TPolicy policy, TResource resource, IUserContextProvider<TUser> userContextProvider, IPolicyDataProvider<TUser> dataProvider)
        {
            constructor = GetCtor(policy);

            return constructor(policy, resource, userContextProvider, dataProvider);
        }

        public static Constructor GetCtor(TPolicy policy)
            => constructor ??= BuildCtor(policy.DataType);
        
        private static Constructor BuildCtor(Type policyDataType)
        {
            var providerType = typeof(PolicyContextProvider<,,,>).MakeGenericType(typeof(TPolicy), typeof(TUser), typeof(TResource), policyDataType);

            var ctor = providerType.GetConstructors().Single();

            var parameters = new[]
            {
                Expression.Parameter(typeof(TPolicy)),
                Expression.Parameter(typeof(TResource)),
                Expression.Parameter(typeof(IUserContextProvider<TUser>)),
                Expression.Parameter(typeof(IPolicyDataProvider<TUser>))
            };

            var body = Expression.Convert(Expression.New(ctor, parameters), typeof(IPolicyContextProvider<TUser, TPolicy, TResource>));

            return Expression.Lambda<Constructor>(body, parameters).Compile();
        }
    }
}
