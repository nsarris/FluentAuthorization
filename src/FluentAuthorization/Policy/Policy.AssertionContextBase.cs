namespace FluentAuthorization
{
    public abstract partial class Policy<TUser, TResource, TData>
    {
        /// <summary>
        /// Base implementation of an assertion context.
        /// </summary>
        public abstract class AssertionContextBase
        {
            protected AssertionContextBase(
                TUser user, 
                TResource resource, 
                TData data, 
                string permissionName, 
                string policyName)
            {
                Data = data;
                Resource = resource;
                User = user;
                PermissionName = permissionName;
                PolicyName = policyName;
            }

            /// <summary>
            /// The user context related to the assertion context.
            /// </summary>
            public TUser User { get; set; }

            /// <summary>
            /// The resource related to the assertion context.
            /// </summary>
            public TResource Resource { get; }

            /// <summary>
            /// The policy data of the current assertion context.
            /// </summary>
            public TData Data { get; }

            /// <summary>
            /// The name of the asserted permission .
            /// </summary>
            public string PermissionName { get; }

            /// <summary>
            /// The name of the asserted policy.
            /// </summary>
            public string PolicyName { get; }

            /// <summary>
            /// Produces an Allow result.
            /// </summary>
            /// <returns>An Allow results</returns>
            public AssertionResult Allow() => AssertionResult.Success;

            /// <summary>
            /// Produces an undefined(null) result.
            /// </summary>
            /// <returns>Null</returns>
            public AssertionResult Undefined() => null;
        }
    }
}
