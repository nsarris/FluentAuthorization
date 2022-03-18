using System;
using System.Linq;
using System.Linq.Expressions;

namespace FluentAuthorization
{
    internal delegate IPolicyContextProvider<TUser, TPolicy> Constructor<TUser, TPolicy>(
            TPolicy policy,
            IUserContextProvider<TUser> userContextProvider,
            IPolicyDataProvider<TUser> policyDataProvider)
        where TPolicy : class, IPolicy<TUser>, new();

    internal static class PolicyContextProviderFactory<TUser, TPolicy>
        where TPolicy : class, IPolicy<TUser>, new()
    {
        private static Constructor<TUser, TPolicy> constructor;

        public static IPolicyContextProvider<TUser, TPolicy> Build(TPolicy policy, IUserContextProvider<TUser> userContextProvider, IPolicyDataProvider<TUser> dataProvider)
        {
            constructor = GetCtor(policy);

            return constructor(policy, userContextProvider, dataProvider);
        }

        public static Constructor<TUser, TPolicy> GetCtor(TPolicy policy)
            => constructor ??= BuildCtor(policy.ResourceType, policy.DataType);
        
        private static Constructor<TUser, TPolicy> BuildCtor(Type resourceDataType, Type policyDataType)
        {
            var providerType = typeof(PolicyContextProvider<,,,>).MakeGenericType(typeof(TPolicy), typeof(TUser), resourceDataType, policyDataType);

            var ctorTypes = new[]
            {
                typeof(TPolicy),
                typeof(IUserContextProvider<TUser>),
                typeof(IPolicyDataProvider<TUser>)
            };

            var ctor = providerType.GetConstructor(ctorTypes);

            var parameters = ctorTypes.Select(Expression.Parameter).ToArray();
            
            var body = Expression.Convert(Expression.New(ctor, parameters), typeof(IPolicyContextProvider<TUser, TPolicy>));

            return Expression.Lambda<Constructor<TUser, TPolicy>>(body, parameters).Compile();
        }
    }
}
