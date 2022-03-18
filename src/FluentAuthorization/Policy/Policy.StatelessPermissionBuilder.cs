using System;
using System.Linq.Expressions;

namespace FluentAuthorization
{
    public abstract partial class Policy<TUser, TResource, TData>
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

            /// <summary>
            /// Defines the name of the permission reported in failure results.
            /// </summary>
            /// <param name="name">The selected name.</param>
            /// <returns></returns>
            public StatelessPermissionBuilder WithName(string name)
            {
                this.name = name;
                return this;
            }

            /// <summary>
            /// Overrides the default assertion failure message.
            /// </summary>
            /// <param name="name">The selected name.</param>
            /// <returns></returns>
            public StatelessPermissionBuilder WithMessage(Func<AssertionContext, string> messageBuilder)
            {
                this.messageBuilder = messageBuilder;
                return this;
            }

            /// <summary>
            /// Builds the permission.
            /// </summary>
            /// <returns></returns>
            public Permission Build()
                => new InlinePermission(assert, name, messageBuilder);
        }
    }
}
