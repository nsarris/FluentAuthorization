using System;

namespace FluentAuthorization
{
    public abstract partial class Policy<TUser, TResource, T>
    {
        public class PolicyBuilderRoot
        {
            public PolicyBuilder AssertWith(Func<AssertionContext, AssertionResult> assert)
            {
                return new PolicyBuilder(assert);
            }

            public PolicyBuilder AssertWith(Func<AssertionContext, bool> assert)
            {
                return new PolicyBuilder(ctx => assert(ctx) ? ctx.Allow() : ctx.Deny());
            }

            public PolicyBuilder<TState> AssertWith<TState>(Func<AssertionContext<TState>, AssertionResult> assert)
            {
                return new PolicyBuilder<TState>(assert);
            }

            public PolicyBuilder<TState> AssertWith<TState>(Func<AssertionContext<TState>, bool> assert)
            {
                return new PolicyBuilder<TState>(ctx => assert(ctx) ? ctx.Allow() : ctx.Deny());
            }
        }
    }
}
