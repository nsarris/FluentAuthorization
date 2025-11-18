using System.Threading.Tasks;

namespace FluentAuthorization
{
    public abstract partial class Policy<TUser, TResource, TData>
    {
        internal interface IAsyncPermission
        {
            Task<AssertionResult> AssertAsync(AssertionContext context);
            string BuildMessage(AssertionContext context);
            string Name { get; }
        }

        /// <summary>
        /// Abstract asynchronous policy permission. Inherit from this class to define your own async permission classes if you opt out using the inline delegate based permission builder.
        /// </summary>
        public abstract class AsyncPermission : FluentAuthorization.IAsyncPermission, IAsyncPermission
        {
            protected abstract Task<AssertionResult> AssertAsync(AssertionContext context);
            protected abstract string BuildMessage(AssertionContext context);
            public abstract string Name { get; }

            public override string ToString() => Name;

            Task<AssertionResult> IAsyncPermission.AssertAsync(AssertionContext context) => AssertAsync(context);
            string IAsyncPermission.BuildMessage(AssertionContext context) => BuildMessage(context);
        }
    }
}
