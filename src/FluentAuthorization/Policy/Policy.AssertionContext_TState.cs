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
            private readonly IAsyncPermission<TState> asyncPermission;

            internal AssertionContext(TUser user, TResource resource, TData data, TState state, IPermission<TState> permission, string policyName)
                : base(user, resource, data, permission.Name, policyName)
            {
                State = state;
                this.permission = permission;
            }

            internal AssertionContext(TUser user, TResource resource, TData data, TState state, IAsyncPermission<TState> asyncPermission, string policyName)
                : base(user, resource, data, asyncPermission.Name, policyName)
            {
                State = state;
                this.asyncPermission = asyncPermission;
            }

            public TState State { get; }

            private AssertionFailure BuildFailure(string reason)
                => new AssertionFailure(User.ToString(), PermissionName, PolicyName, BuildMessageInternal(), reason);

            private string BuildMessageInternal()
            {
                if (permission != null)
                    return permission.BuildMessage(this);
                if (asyncPermission != null)
                    return asyncPermission.BuildMessage(this);
                return string.Empty;
            }

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
