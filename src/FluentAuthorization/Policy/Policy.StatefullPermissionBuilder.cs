using System;

namespace FluentAuthorization
{
    public abstract partial class Policy<TUser, TResource, TData>
    {
        public class StatefullPermissionBuilder<TState>
        {
            private readonly Func<AssertionContext<TState>, AssertionResult> assert;
            private Func<AssertionContext<TState>, string> messageBuilder;
            private string name;

            internal StatefullPermissionBuilder(Func<AssertionContext<TState>, AssertionResult> assert)
            {
                this.assert = assert;
            }

            /// <summary>
            /// Defines the name of the permission reported in failure results.
            /// </summary>
            /// <param name="name">The selected name.</param>
            /// <returns></returns>
            public StatefullPermissionBuilder<TState> WithName(string name)
            {
                this.name = name;
                return this;
            }

            /// <summary>
            /// Overrides the default assertion failure message.
            /// </summary>
            /// <param name="name">The selected name.</param>
            /// <returns></returns>
            public StatefullPermissionBuilder<TState> WithMessage(Func<AssertionContext<TState>, string> messageBuilder)
            {
                this.messageBuilder = messageBuilder;
                return this;
            }

            /// <summary>
            /// Builds the permission.
            /// </summary>
            /// <returns></returns>
            public Permission<TState> Build()
                => new InlinePermission<TState>(assert, name, messageBuilder);
        }
    }
}
