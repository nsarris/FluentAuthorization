using System;

namespace FluentAuthorization
{
    public static class PolicyContextExtensions
    {
        public static TData GetData<TData>(this IPolicyContext<IPolicyWithData<TData>> context)
        {
            var typedContext = (IPolicyContextDataInternal<TData>)context;

            return typedContext.Data;
        }

        public static void ThrowOnDeny<T>(this IPolicyContext<T> policy, Func<T, IPermission> select)
            where T : IPolicy
        {
            policy.Assert(select).ThowOnDeny();
        }

        public static void ThrowOnDeny<T, TState>(this IPolicyContext<T> policy, Func<T, IPermission<TState>> select, TState state)
            where T : IPolicy
        {
            policy.Assert(select, state).ThowOnDeny();
        }
    }
}
