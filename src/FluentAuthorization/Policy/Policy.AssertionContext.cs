using System.Collections.Generic;
using System.Linq;

namespace FluentAuthorization
{
    public abstract partial class Policy<TUser, TResource, TData>
    {
        /// <summary>
        /// A permission assertion context implementation.
        /// </summary>
        public class AssertionContext : AssertionContextBase
        {
            private readonly IPermission permission;
            private readonly IAsyncPermission asyncPermission;

            internal AssertionContext(TUser user, TResource resource, TData data, IPermission permission, string policyName) 
                : base(user, resource, data, permission.Name, policyName)
            {
                this.permission = permission;
            }

            internal AssertionContext(TUser user, TResource resource, TData data, IAsyncPermission asyncPermission, string policyName) 
                : base(user, resource, data, asyncPermission.Name, policyName)
            {
                this.asyncPermission = asyncPermission;
            }

            private AssertionFailure BuildFailure(string reason)
                => new (User.ToString(), PermissionName, PolicyName, BuildMessageInternal(), reason);

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
