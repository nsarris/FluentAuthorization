using System;

namespace FluentAuthorization
{
    public abstract partial class Policy<TUser, TResource, T>
    {
        public class PolicyBuilder<TState>
        {
            private readonly Func<AssertionContext<TState>, AssertionResult> assert;
            private Func<AssertionContext<TState>, string> messageBuilder;
            private string name;

            public PolicyBuilder(Func<AssertionContext<TState>, AssertionResult> assert)
            {
                this.assert = assert;
            }

            public PolicyBuilder<TState> WithName(string name)
            {
                this.name = name;
                return this;
            }

            public PolicyBuilder<TState> WithMessage(Func<AssertionContext<TState>, string> messageBuilder)
            {
                this.messageBuilder = messageBuilder;
                return this;
            }

            public Permission<TState> Build()
                => new InlinePermission<TState>(assert, name, messageBuilder);
        }
    }
}
