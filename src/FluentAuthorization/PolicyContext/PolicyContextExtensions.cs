using System;
using System.Collections.Generic;

namespace FluentAuthorization
{
    public static class PolicyContextExtensions
    {
        public static IEnumerable<TData> GetData<TData>(this IPolicyContext<IPolicyWithData<TData>> context)
        {
            var typedContext = (IDataContainer<TData>)context;

            return typedContext.Data;
        }

        public static void ThrowOnDeny<T>(this IPolicyContext<T> policy, Func<T, IPermission> select)
            where T : IPolicy
        {
            policy.Assert(select).ThrowOnDeny();
        }

        public static void ThrowOnDeny<T, TState>(this IPolicyContext<T> policy, Func<T, IPermission<TState>> select, TState state)
            where T : IPolicy
        {
            policy.Assert(select, state).ThrowOnDeny();
        }
    }
}
