using System;

namespace FluentAuthorization
{
    public abstract partial class Policy<TUser, TResource, TData>
    {
        /// <summary>
        /// Fluent permission builder for the container policy.
        /// </summary>
        public class PermissionBuilder
        {
            /// <summary>
            /// Defines the permission assertion logic for a stateless permission.
            /// </summary>
            /// <param name="assert">The assertion function.</param>
            /// <returns></returns>
            public StatelessPermissionBuilder AssertWith(Func<AssertionContext, AssertionResult> assert)
            {
                return new StatelessPermissionBuilder(assert);
            }

            /// <summary>
            /// Defines the permission assertion logic for a stateless permission.
            /// </summary>
            /// <param name="assert">The assertion function.</param>
            /// <returns></returns>
            public StatelessPermissionBuilder AssertWith(Func<AssertionContext, bool> assert)
            {
                return new StatelessPermissionBuilder(ctx => assert(ctx) ? ctx.Allow() : ctx.Deny());
            }

            /// <summary>
            /// Defines the permission assertion logic for a stateless permission.
            /// </summary>
            /// <param name="assert">The assertion function.</param>
            /// <returns></returns>
            public StatelessPermissionBuilder AssertWith(Func<AssertionContext, bool?> assert)
            {
                return new StatelessPermissionBuilder(ctx => assert(ctx) switch
                {
                    true => ctx.Allow(),
                    false => ctx.Deny(),
                    _ => ctx.Undefined()
                });
            }

            /// <summary>
            /// Defines the permission assertion logic for a statefull permission.
            /// </summary>
            /// <param name="assert">The assertion function.</param>
            /// <returns></returns>
            public StatefullPermissionBuilder<TState> AssertWith<TState>(Func<AssertionContext<TState>, AssertionResult> assert)
            {
                return new StatefullPermissionBuilder<TState>(assert);
            }

            /// <summary>
            /// Defines the permission assertion logic for a statefull permission.
            /// </summary>
            /// <param name="assert">The assertion function.</param>
            /// <returns></returns>
            public StatefullPermissionBuilder<TState> AssertWith<TState>(Func<AssertionContext<TState>, bool> assert)
            {
                return new StatefullPermissionBuilder<TState>(ctx => assert(ctx) ? ctx.Allow() : ctx.Deny());
            }

            /// <summary>
            /// Defines the permission assertion logic for a statefull permission.
            /// </summary>
            /// <param name="assert">The assertion function.</param>
            /// <returns></returns>
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
