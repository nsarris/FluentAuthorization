using System;

namespace FluentAuthorization
{
    public abstract partial class Policy<TUser, TResource, T>
    {
        public class PolicyBuilder
        {
            private readonly Func<AssertionContext, AssertionResult> assert;
            private Func<AssertionContext, string> messageBuilder;
            private string name;

            public PolicyBuilder(Func<AssertionContext, AssertionResult> assert)
            {
                this.assert = assert;
            }

            public PolicyBuilder WithName(string name)
            {
                this.name = name;
                return this;
            }

            public PolicyBuilder WithMessage(Func<AssertionContext, string> messageBuilder)
            {
                this.messageBuilder = messageBuilder;
                return this;
            }

            public Permission Build()
                => new InlinePermission(assert, name, messageBuilder);
        }
    }
}
