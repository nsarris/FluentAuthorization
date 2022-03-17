using System.Collections.Generic;
using System.Linq;

namespace FluentAuthorization
{
    public abstract partial class Policy<TUser, TResource, TData>
    {
        public class AssertionContext<TState> : AssertionContextBase
        {
            private readonly IPermission<TState> permission;

            internal AssertionContext(TUser user, TResource resource, TData data, TState state, IPermission<TState> permission, string policyName)
                : base(user, resource, data, permission.Name, policyName)
            {
                State = state;
                this.permission = permission;
            }

            public TState State { get; }

            private AssertionFailure BuildFailure(string reason)
                => new AssertionFailure(User.ToString(), PermissionName, PolicyName, permission.BuildMessage(this), reason);

            public AssertionResult Deny(string reason = null) => new(BuildFailure(reason));
            public AssertionResult Deny(IEnumerable<string> reasons) => new(reasons.Select(BuildFailure));
        }
    }
}
