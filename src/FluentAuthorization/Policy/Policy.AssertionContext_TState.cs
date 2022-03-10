namespace FluentAuthorization
{
    public abstract partial class Policy<TUser, TResource, T>
    {
        public class AssertionContext<TState> : AssertionContextBase
        {
            private readonly IPermission<TState> permission;

            internal AssertionContext(TUser user, TResource resource, T data, TState state, IPermission<TState> permission, string policyName)
                : base(user, resource, data, permission.Name, policyName)
            {
                State = state;
                this.permission = permission;
            }

            public TState State { get; }

            public AssertionResult Deny(string reason = null) => new(new AssertionFailure(User.ToString(), PermissionName, PolicyName, permission.BuildMessage(this), reason));
        }
    }
}
