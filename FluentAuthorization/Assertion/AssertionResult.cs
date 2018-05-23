using System.Collections.Generic;
using System.Linq;

namespace FluentAuthorization
{
    public class AssertionResult
    {
        public bool Allow { get; private set; }
        public bool Deny => !Allow;
        public string DenialMessage { get; private set; }
        
        public IEnumerable<EffectivePermission> DeniedPermissions { get; private set; }

        internal AssertionResult(bool allow, IEnumerable<EffectivePermission> denialPermissions, string message)
        {
            Allow = allow;
            DenialMessage = message;
            if (!allow)
                DeniedPermissions = denialPermissions.ToList();
            else
                DeniedPermissions = new List<EffectivePermission>();
        }

        internal AssertionResult(EffectivePermission PermissionResult)
        {
            Allow = PermissionResult.Allow;
            DenialMessage = PermissionResult.DenialMessage;
            DeniedPermissions = new[] { PermissionResult };
        }
    }
}
