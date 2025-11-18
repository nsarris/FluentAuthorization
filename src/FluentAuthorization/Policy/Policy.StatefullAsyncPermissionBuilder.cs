using System;
using System.Threading.Tasks;

namespace FluentAuthorization
{
    public abstract partial class Policy<TUser, TResource, TData>
    {
        public class StatefullAsyncPermissionBuilder<TState>
        {
            private readonly Func<AssertionContext<TState>, Task<AssertionResult>> assertAsync;
            private Func<AssertionContext<TState>, string> messageBuilder;
            private string name;

            internal StatefullAsyncPermissionBuilder(Func<AssertionContext<TState>, Task<AssertionResult>> assertAsync)
            {
                this.assertAsync = assertAsync;
            }

            /// <summary>
            /// Defines the name of the permission reported in failure results.
            /// </summary>
            /// <param name="name">The selected name.</param>
            /// <returns></returns>
            public StatefullAsyncPermissionBuilder<TState> WithName(string name)
            {
                this.name = name;
                return this;
            }

            /// <summary>
            /// Overrides the default assertion failure message.
            /// </summary>
            /// <param name="messageBuilder">The message builder function.</param>
            /// <returns></returns>
            public StatefullAsyncPermissionBuilder<TState> WithMessage(Func<AssertionContext<TState>, string> messageBuilder)
            {
                this.messageBuilder = messageBuilder;
                return this;
            }

            /// <summary>
            /// Builds the async permission.
            /// </summary>
            /// <returns></returns>
            public AsyncPermission<TState> Build()
                => new InlineAsyncPermission<TState>(assertAsync, name, messageBuilder);
        }
    }
}
