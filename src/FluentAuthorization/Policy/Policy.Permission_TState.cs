﻿namespace FluentAuthorization
{
    public abstract partial class Policy<TUser, TResource, T>
    {
        internal interface IPermission<TState>
        {
            AssertionResult Assert(AssertionContext<TState> context);
            string BuildMessage(AssertionContext<TState> context);
            string Name { get; }
        }

        public abstract class Permission<TState> : FluentAuthorization.IPermission<TState>, IPermission<TState>
        {
            protected abstract AssertionResult Assert(AssertionContext<TState> context);
            protected abstract string BuildMessage(AssertionContext<TState> context);
            public abstract string Name { get; }

            public override string ToString() => Name;

            AssertionResult IPermission<TState>.Assert(AssertionContext<TState> context) => Assert(context);
            string IPermission<TState>.BuildMessage(AssertionContext<TState> context) => BuildMessage(context);
        }
    }
}
