using System;

namespace FluentAuthorization
{
    public abstract partial class Policy<TUser, TResource, T>
    {
        internal class InlinePermission : Permission
        {
            private readonly Func<AssertionContext, AssertionResult> assert;
            private readonly Func<AssertionContext, string> messageBuilder;
            
            public InlinePermission(
                Func<AssertionContext, AssertionResult> assert,
                string name,
                Func<AssertionContext, string> messageBuilder)
            {
                this.assert = assert;
                this.messageBuilder = messageBuilder ?? BuildDefaultMessage;
                Name = name;
            }

            public override string Name { get; }

            protected override AssertionResult Assert(AssertionContext context)
                => assert(context);

            protected override string BuildMessage(AssertionContext context)
                => messageBuilder(context);
        }
    }
}
