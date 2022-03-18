using System;
using System.Collections.Generic;

namespace FluentAuthorization
{
    public static class PolicyContextExtensions
    {
        /// <summary>
        /// Gets the encapsulated data of the source policy context.
        /// </summary>
        /// <typeparam name="TData">The type of data defined by the encapsulated policy.</typeparam>
        /// <param name="context">The source context.</param>
        /// <returns>The collection of data encapsulated.</returns>
        public static IEnumerable<TData> GetData<TData>(this IPolicyContext<IPolicyWithData<TData>> context)
        {
            var typedContext = (IDataContainer<TData>)context;

            return typedContext.Data;
        }

        /// <summary>
        /// Gets an aggregated instance the encapsulated data of the source policy context.
        /// </summary>
        /// <typeparam name="TData">The type of data defined by the encapsulated policy.</typeparam>
        /// <param name="context">The source context.</param>
        /// <returns>An aggregated instance data encapsulated.</returns>
        public static TData GetAggregatedData<TData>(this IPolicyContext<IPolicyWithData<TData>> context)
        {
            var typedContext = (IDataContainer<TData>)context;

            return context.Policy.Aggregate(typedContext.Data);
        }

        /// <summary>
        /// Throw a PolicyAssertionException if the assertion of the selected permission fails.
        /// </summary>
        /// <typeparam name="T">The type of the policy.</typeparam>
        /// <param name="context">The source context.</param>
        /// <param name="select">The permission selector.</param>
        public static void ThrowOnDeny<T>(this IPolicyContext<T> context, Func<T, IPermission> select)
            where T : IPolicy
        {
            context.Assert(select).ThrowOnDeny();
        }

        /// <summary>
        /// Throw a PolicyAssertionException if the assertion of the selected permission fails.
        /// </summary>
        /// <typeparam name="T">The type of the policy.</typeparam>
        /// <param name="context">The source context.</param>
        /// <param name="select">The permission selector.</param>
        public static void ThrowOnDeny<T, TState>(this IPolicyContext<T> policy, Func<T, IPermission<TState>> select, TState state)
            where T : IPolicy
        {
            policy.Assert(select, state).ThrowOnDeny();
        }
    }
}
