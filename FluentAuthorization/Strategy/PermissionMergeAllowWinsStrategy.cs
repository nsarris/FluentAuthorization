using System;
using System.Linq;
using System.Collections.Generic;

namespace FluentAuthorization
{
    public class PermissionMergeAllowWinsStrategy : IPermissionReduceStrategy
    {
        readonly static Lazy<PermissionMergeAllowWinsStrategy> instance = new Lazy<PermissionMergeAllowWinsStrategy>(() => new PermissionMergeAllowWinsStrategy());
        internal static PermissionMergeAllowWinsStrategy Instance => instance.Value;
        public AssertionResult Reduce(IEnumerable<AssertionResult> Permissions)
        {
            var deniedPermissions = Permissions.Where(x => x.Deny).ToList();

            return new AssertionResult(deniedPermissions.Count != Permissions.Count(),
                deniedPermissions.SelectMany(x => x.DeniedPermissions),
                string.Join(Environment.NewLine,
                    deniedPermissions.Select(x => x.DenialMessage)));
        }
    }

}
