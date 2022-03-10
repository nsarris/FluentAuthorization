using System;
using System.Linq.Expressions;

namespace FluentAuthorization
{
    public abstract partial class Policy<TUser, TResource, T>
    {
        public class StatelessPermissionBuilder
        {
            private readonly Func<AssertionContext, AssertionResult> assert;
            private Func<AssertionContext, string> messageBuilder;
            private string name;

            public StatelessPermissionBuilder(Func<AssertionContext, AssertionResult> assert)
            {
                this.assert = assert;
            }

            public StatelessPermissionBuilder WithName(string name)
            {
                this.name = name;
                return this;
            }

            public StatelessPermissionBuilder WithMessage(Func<AssertionContext, string> messageBuilder)
            {
                this.messageBuilder = messageBuilder;
                return this;
            }

            public Permission Build()
                => new InlinePermission(assert, name, messageBuilder);
        }
    }
}
