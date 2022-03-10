using System;

namespace FluentAuthorization
{
    public abstract partial class Policy<TUser, TResource, T>
    {
        public class StatefullPermissionBuilder<TState>
        {
            private readonly Func<AssertionContext<TState>, AssertionResult> assert;
            private Func<AssertionContext<TState>, string> messageBuilder;
            private string name;

            public StatefullPermissionBuilder(Func<AssertionContext<TState>, AssertionResult> assert)
            {
                this.assert = assert;
            }

            public StatefullPermissionBuilder<TState> WithName(string name)
            {
                this.name = name;
                return this;
            }

            public StatefullPermissionBuilder<TState> WithMessage(Func<AssertionContext<TState>, string> messageBuilder)
            {
                this.messageBuilder = messageBuilder;
                return this;
            }

            public Permission<TState> Build()
                => new InlinePermission<TState>(assert, name, messageBuilder);
        }
    }
}
