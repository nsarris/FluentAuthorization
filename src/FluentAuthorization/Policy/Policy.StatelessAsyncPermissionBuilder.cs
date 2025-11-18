using System;
using System.Threading.Tasks;

namespace FluentAuthorization
{
    public abstract partial class Policy<TUser, TResource, TData>
    {
        public class StatelessAsyncPermissionBuilder
        {
            private readonly Func<AssertionContext, Task<AssertionResult>> assertAsync;
            private Func<AssertionContext, string> messageBuilder;
            private string name;

            public StatelessAsyncPermissionBuilder(Func<AssertionContext, Task<AssertionResult>> assertAsync)
            {
                this.assertAsync = assertAsync;
            }

            /// <summary>
            /// Defines the name of the permission reported in failure results.
            /// </summary>
            /// <param name="name">The selected name.</param>
            /// <returns></returns>
            public StatelessAsyncPermissionBuilder WithName(string name)
            {
                this.name = name;
                return this;
            }

            /// <summary>
            /// Overrides the default assertion failure message.
            /// </summary>
            /// <param name="messageBuilder">The message builder function.</param>
            /// <returns></returns>
            public StatelessAsyncPermissionBuilder WithMessage(Func<AssertionContext, string> messageBuilder)
            {
                this.messageBuilder = messageBuilder;
                return this;
            }

            /// <summary>
            /// Builds the async permission.
            /// </summary>
            /// <returns></returns>
            public AsyncPermission Build()
                => new InlineAsyncPermission(assertAsync, name, messageBuilder);
        }
    }
}
