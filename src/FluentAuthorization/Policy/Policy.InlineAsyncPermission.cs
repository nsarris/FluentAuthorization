using System;
using System.Threading.Tasks;

namespace FluentAuthorization
{
    public abstract partial class Policy<TUser, TResource, TData>
    {
        internal class InlineAsyncPermission : AsyncPermission
        {
            private readonly Func<AssertionContext, Task<AssertionResult>> assertAsync;
            private readonly Func<AssertionContext, string> messageBuilder;
            
            public InlineAsyncPermission(
                Func<AssertionContext, Task<AssertionResult>> assertAsync,
                string name,
                Func<AssertionContext, string> messageBuilder)
            {
                this.assertAsync = assertAsync;
                this.messageBuilder = messageBuilder ?? BuildDefaultMessage;
                Name = name;
            }

            public override string Name { get; }

            protected override Task<AssertionResult> AssertAsync(AssertionContext context)
                => assertAsync(context);

            protected override string BuildMessage(AssertionContext context)
                => messageBuilder(context);
        }
    }
}
