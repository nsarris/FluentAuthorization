namespace FluentAuthorization
{
    public abstract partial class Policy<TUser, TResource, T>
    {
        public abstract class Permission : IPermission
        {
            public abstract AssertionResult Assert(AssertionContext context);
            public abstract string BuildMessage(AssertionContext context);
            public abstract string Name { get; }

            public override string ToString() => Name;
        }
    }
}
