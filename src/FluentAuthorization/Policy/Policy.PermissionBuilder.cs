using System;

namespace FluentAuthorization
{
    public abstract partial class Policy<TUser, TResource, T>
    {
        public class PermissionBuilder
        {
            public StatelessPermissionBuilder AssertWith(Func<AssertionContext, AssertionResult> assert)
            {
                return new StatelessPermissionBuilder(assert);
            }

            public StatelessPermissionBuilder AssertWith(Func<AssertionContext, bool> assert)
            {
                return new StatelessPermissionBuilder(ctx => assert(ctx) ? ctx.Allow() : ctx.Deny());
            }

            public StatelessPermissionBuilder AssertWith(Func<AssertionContext, bool?> assert)
            {
                return new StatelessPermissionBuilder(ctx => assert(ctx) switch
                {
                    true => ctx.Allow(),
                    false => ctx.Deny(),
                    _ => ctx.Undefined()
                });
            }

            public StatefullPermissionBuilder<TState> AssertWith<TState>(Func<AssertionContext<TState>, AssertionResult> assert)
            {
                return new StatefullPermissionBuilder<TState>(assert);
            }

            public StatefullPermissionBuilder<TState> AssertWith<TState>(Func<AssertionContext<TState>, bool> assert)
            {
                return new StatefullPermissionBuilder<TState>(ctx => assert(ctx) ? ctx.Allow() : ctx.Deny());
            }

            public StatefullPermissionBuilder<TState> AssertWith<TState>(Func<AssertionContext<TState>, bool?> assert)
            {
                return new StatefullPermissionBuilder<TState>(ctx => assert(ctx) switch
                {
                    true => ctx.Allow(),
                    false => ctx.Deny(),
                    _ => ctx.Undefined()
                });
            }
        }
    }
}
