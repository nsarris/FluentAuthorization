using System;
using System.Threading.Tasks;

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

            /// <summary>
            /// Defines the asynchronous permission assertion logic for a stateless permission.
            /// </summary>
            /// <param name="assertAsync">The async assertion function.</param>
            /// <returns></returns>
            public StatelessAsyncPermissionBuilder AssertWithAsync(Func<AssertionContext, Task<AssertionResult>> assertAsync)
            {
                return new StatelessAsyncPermissionBuilder(assertAsync);
            }

            /// <summary>
            /// Defines the asynchronous permission assertion logic for a stateless permission.
            /// </summary>
            /// <param name="assertAsync">The async assertion function.</param>
            /// <returns></returns>
            public StatelessAsyncPermissionBuilder AssertWithAsync(Func<AssertionContext, Task<bool>> assertAsync)
            {
                return new StatelessAsyncPermissionBuilder(async ctx => await assertAsync(ctx) ? ctx.Allow() : ctx.Deny());
            }

            /// <summary>
            /// Defines the asynchronous permission assertion logic for a stateless permission.
            /// </summary>
            /// <param name="assertAsync">The async assertion function.</param>
            /// <returns></returns>
            public StatelessAsyncPermissionBuilder AssertWithAsync(Func<AssertionContext, Task<bool?>> assertAsync)
            {
                return new StatelessAsyncPermissionBuilder(async ctx => (await assertAsync(ctx)) switch
                {
                    true => ctx.Allow(),
                    false => ctx.Deny(),
                    _ => ctx.Undefined()
                });
            }

            /// <summary>
            /// Defines the asynchronous permission assertion logic for a statefull permission.
            /// </summary>
            /// <param name="assertAsync">The async assertion function.</param>
            /// <returns></returns>
            public StatefullAsyncPermissionBuilder<TState> AssertWithAsync<TState>(Func<AssertionContext<TState>, Task<AssertionResult>> assertAsync)
            {
                return new StatefullAsyncPermissionBuilder<TState>(assertAsync);
            }

            /// <summary>
            /// Defines the asynchronous permission assertion logic for a statefull permission.
            /// </summary>
            /// <param name="assertAsync">The async assertion function.</param>
            /// <returns></returns>
            public StatefullAsyncPermissionBuilder<TState> AssertWithAsync<TState>(Func<AssertionContext<TState>, Task<bool>> assertAsync)
            {
                return new StatefullAsyncPermissionBuilder<TState>(async ctx => await assertAsync(ctx) ? ctx.Allow() : ctx.Deny());
            }

            /// <summary>
            /// Defines the asynchronous permission assertion logic for a statefull permission.
            /// </summary>
            /// <param name="assertAsync">The async assertion function.</param>
            /// <returns></returns>
            public StatefullAsyncPermissionBuilder<TState> AssertWithAsync<TState>(Func<AssertionContext<TState>, Task<bool?>> assertAsync)
            {
                return new StatefullAsyncPermissionBuilder<TState>(async ctx => (await assertAsync(ctx)) switch
                {
                    true => ctx.Allow(),
                    false => ctx.Deny(),
                    _ => ctx.Undefined()
                });
            }
        }
    }
}
