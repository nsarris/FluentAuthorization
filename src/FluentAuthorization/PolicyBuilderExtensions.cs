using System;
using System.Linq.Expressions;

namespace FluentAuthorization
{
    public static class PolicyBuilderExtensions
    {
        public static PolicyBuilder<TUser, TResource, T> For<TUser, TResource, T, TPolicy>(this TPolicy p, Expression<Func<TPolicy, Policy<TUser, TResource, T>.Permission>> select) where TPolicy : Policy<TUser, TResource, T>
        {
            return new PolicyBuilder<TUser, TResource, T>();
        }

        public static PolicyBuilder<TUser, TResource, T, TState> For<TUser, TResource, T, TPolicy, TState>(this TPolicy p, Expression<Func<TPolicy, Policy<TUser, TResource, T>.Permission<TState>>> select) where TPolicy : Policy<TUser, TResource, T>
        {
            return new PolicyBuilder<TUser, TResource, T, TState>();
        }
    }
}
