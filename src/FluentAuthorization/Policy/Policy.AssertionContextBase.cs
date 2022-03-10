namespace FluentAuthorization
{
    public abstract partial class Policy<TUser, TResource, T>
    {
        public abstract class AssertionContextBase
        {
            protected AssertionContextBase(TUser user, TResource resource, T data, string permissionName, string policyName)
            {
                Data = data;
                Resource = resource;
                User = user;
                PermissionName = permissionName;
                PolicyName = policyName;
            }

            public TUser User { get; set; }
            public TResource Resource { get; }
            public T Data { get; }
            public string PermissionName { get; }
            public string PolicyName { get; }

            public AssertionResult Allow() => AssertionResult.Success;
            //public AssertionResult Deny(string reason = null) => new(new AssertionFailure(PermissionName, PolicyName, reason));

            public AssertionResult Undefined() => null;
        }
    }
}
