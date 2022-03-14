﻿using System.Collections.Generic;
using System.Linq;

namespace FluentAuthorization
{
    public abstract partial class Policy<TUser, TResource, T>
    {
        public class AssertionContext : AssertionContextBase
        {
            private readonly IPermission permission;

            internal AssertionContext(TUser user, TResource resource, T data, IPermission permission, string policyName) 
                : base(user, resource, data, permission.Name, policyName)
            {
                this.permission = permission;
            }

            public AssertionResult Deny(string reason = null) => new(new AssertionFailure(User.ToString(), PermissionName, PolicyName, permission.BuildMessage(this), reason));
            public AssertionResult Deny(IEnumerable<string> reasons) => new(false, reasons.Select(r => new AssertionFailure(User.ToString(), PermissionName, PolicyName, permission.BuildMessage(this), r)));
        }
    }
}
