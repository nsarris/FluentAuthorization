using System;
using System.Linq;
using System.Collections.Generic;

namespace FluentAuthorization
{
    public class PermissionMergeDenyWinsStrategy : IPermissionReduceStrategy
    {
        readonly static Lazy<PermissionMergeDenyWinsStrategy> instance = new Lazy<PermissionMergeDenyWinsStrategy>(() => new PermissionMergeDenyWinsStrategy());
        internal static PermissionMergeDenyWinsStrategy Instance => instance.Value;
        public AssertionResult Reduce(IEnumerable<AssertionResult> Permissions)
        {
            var deniedPermissions = Permissions.Where(x => x.Deny).ToList();
            
            return new AssertionResult(!deniedPermissions.Any(), 
                deniedPermissions.SelectMany(x => x.DeniedPermissions), 
                string.Join(Environment.NewLine, 
                    deniedPermissions.Select(x => x.DenialMessage)));
        }
    }

}
