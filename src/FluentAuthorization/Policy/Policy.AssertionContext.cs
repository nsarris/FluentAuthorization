﻿namespace FluentAuthorization
{
    public abstract partial class Policy<TUser, TResource, T>
    {
        public class AssertionContext : AssertionContextBase
        {
            private readonly Permission permission;

            internal AssertionContext(TUser user, TResource resource, T data, Permission permission, string policyName) 
                : base(user, resource, data, permission.Name, policyName)
            {
                this.permission = permission;
            }

            public AssertionResult Deny(string reason = null) => new(new AssertionFailure(User.ToString(), PermissionName, PolicyName, permission.BuildMessage(this), reason));
        }
    }
}
