using System.Collections.Generic;
using System.Linq;

namespace FluentAuthorization
{
    public abstract partial class Policy<TUser, TResource, TData>
    {
        /// <summary>
        /// A state handling permission assertion context implementation.
        /// </summary>
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

            /// <summary>
            /// Produces a Deny result with the specified reason.
            /// </summary>
            /// <param name="reason">The reason for the permssion failure.</param>
            /// <returns>A Deny result with the specified reason.</returns>
            public AssertionResult Deny(string reason = null) => new(BuildFailure(reason));

            /// <summary>
            /// Produces a Deny result with the specified reasons.
            /// </summary>
            /// <param name="reason">The reasons for the permssion failure.</param>
            /// <returns>A Deny result with the specified reasons.</returns>
            public AssertionResult Deny(IEnumerable<string> reasons) => new(reasons.Select(BuildFailure));
        }
    }
}
