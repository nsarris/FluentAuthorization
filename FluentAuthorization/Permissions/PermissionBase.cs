using System.Reflection;

namespace FluentAuthorization
{
    public partial class SecurityPolicy<TUserSecurityContext>
    {
        public abstract class PermissionBase : IPermission
        {
            public SecurityPolicy<TUserSecurityContext> Policy { get; }
            public string Name { get; protected set; }
            ISecurityPolicy FluentAuthorization.IPermission.Policy => Policy;
            
            public PermissionBase(SecurityPolicy<TUserSecurityContext> policy)
            {
                Policy = policy;
                Name = this.GetType().GetCustomAttribute<PermissionNameAttribute>()?.Name ?? this.GetType().Name;
            }

            public virtual string GetDenialMessage(TUserSecurityContext user)
            {
                return "Permission " + Name + " of Policy " + Name + " was denied";
            }

            public abstract AssertionResult Assert(TUserSecurityContext user);
        }
    }

    public partial class SecurityPolicy<T, TUserSecurityContext>
    {
        public abstract new class PermissionBase : SecurityPolicy<TUserSecurityContext>.PermissionBase, IPermission
        {
            public new SecurityPolicy<T, TUserSecurityContext> Policy { get; }
            public PermissionBase(SecurityPolicy<T, TUserSecurityContext> policy)
                : base(policy)
            {
                Policy = policy;
            }
        }
    }
}