namespace FluentAuthorization
{
    public abstract partial class Policy<TUser, TResource, TData>
    {
        public abstract class AssertionContextBase
        {
            protected AssertionContextBase(TUser user, TResource resource, TData data, string permissionName, string policyName)
            {
                Data = data;
                Resource = resource;
                User = user;
                PermissionName = permissionName;
                PolicyName = policyName;
            }

            public TUser User { get; set; }
            public TResource Resource { get; }
            public TData Data { get; }
            public string PermissionName { get; }
            public string PolicyName { get; }

            public AssertionResult Allow() => AssertionResult.Success;
            public AssertionResult Undefined() => null;
        }
    }
}
