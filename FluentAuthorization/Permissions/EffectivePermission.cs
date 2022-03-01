namespace FluentAuthorization
{
    public class EffectivePermission
    {
        public bool Allow { get; internal set; }
        public bool Deny => !Allow;
        public IPermission Permission { get; internal set; }
        public ISecurityPolicy Policy { get; internal set; }
        public string DenialMessage { get; internal set; }
        public object Metadata { get; internal set; }
        public object Input { get; internal set; }

        internal EffectivePermission()
        {

        }

        internal EffectivePermission(IPermission Permission, ISecurityPolicy policy, PermissionResult PermissionResult)
        {
            this.Permission = Permission;
            this.Policy = policy;
            this.Input = PermissionResult.Input;
            this.Metadata = PermissionResult.Metadata;
            this.Allow = PermissionResult.Allow;
            this.DenialMessage = PermissionResult.DenialMessage;
        }
    }
}
