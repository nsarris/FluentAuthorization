using System;

namespace FluentAuthorization
{
    public abstract partial class Policy<TUser, TResource, T>
    {
        internal class InlinePermission<TState> : Permission<TState>
        {
            private readonly Func<AssertionContext<TState>, AssertionResult> assert;
            private readonly Func<AssertionContext<TState>, string> messageBuilder;

            public override string Name { get; }

            public InlinePermission(
                Func<AssertionContext<TState>, AssertionResult> assert,
                string name,
                Func<AssertionContext<TState>, string> messageBuilder)
            {
                this.assert = assert;
                this.messageBuilder = messageBuilder ?? BuildDefaultMessage;
                Name = name;
            }

            public override AssertionResult Assert(AssertionContext<TState> context)
                => assert(context);

            public override string BuildMessage(AssertionContext<TState> context)
                => messageBuilder(context);
        }
    }
}
