using System.Threading.Tasks;

namespace FluentAuthorization
{
    public abstract partial class Policy<TUser, TResource, TData>
    {
        internal interface IAsyncPermission<TState>
        {
            Task<AssertionResult> AssertAsync(AssertionContext<TState> context);
            string BuildMessage(AssertionContext<TState> context);
            string Name { get; }
        }

        /// <summary>
        /// Abstract asynchronous policy permission with an input state. Inherit from this class to define your own async permission classes if you opt out using the inline delegate based permission builder.
        /// </summary>
        public abstract class AsyncPermission<TState> : FluentAuthorization.IAsyncPermission<TState>, IAsyncPermission<TState>
        {
            protected abstract Task<AssertionResult> AssertAsync(AssertionContext<TState> context);
            protected abstract string BuildMessage(AssertionContext<TState> context);
            public abstract string Name { get; }

            public override string ToString() => Name;

            Task<AssertionResult> IAsyncPermission<TState>.AssertAsync(AssertionContext<TState> context) => AssertAsync(context);
            string IAsyncPermission<TState>.BuildMessage(AssertionContext<TState> context) => BuildMessage(context);
        }
    }
}
