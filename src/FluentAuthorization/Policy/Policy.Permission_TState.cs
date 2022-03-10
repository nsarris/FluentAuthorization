namespace FluentAuthorization
{
    public abstract partial class Policy<TUser, TResource, T>
    {
        public abstract class Permission<TState> : IPermission<TState>
        {
            public abstract AssertionResult Assert(AssertionContext<TState> context);
            public abstract string BuildMessage(AssertionContext<TState> context);
            public abstract string Name { get; }

            public override string ToString() => Name;
        }
    }
}
