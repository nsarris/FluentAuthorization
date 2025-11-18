using System;
using System.Threading.Tasks;

namespace FluentAuthorization
{
    public abstract partial class Policy<TUser, TResource, TData>
    {
        internal class InlineAsyncPermission<TState> : AsyncPermission<TState>
        {
            private readonly Func<AssertionContext<TState>, Task<AssertionResult>> assertAsync;
            private readonly Func<AssertionContext<TState>, string> messageBuilder;

            public override string Name { get; }

            public InlineAsyncPermission(
                Func<AssertionContext<TState>, Task<AssertionResult>> assertAsync,
                string name,
                Func<AssertionContext<TState>, string> messageBuilder)
            {
                this.assertAsync = assertAsync;
                this.messageBuilder = messageBuilder ?? BuildDefaultMessage;
                Name = name;
            }

            protected override Task<AssertionResult> AssertAsync(AssertionContext<TState> context)
                => assertAsync(context);

            protected override string BuildMessage(AssertionContext<TState> context)
                => messageBuilder(context);
        }
    }
}
