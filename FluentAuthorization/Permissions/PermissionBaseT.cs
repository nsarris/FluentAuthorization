using System.Reflection;

namespace FluentAuthorization
{
    public partial class SecurityPolicy<TUserSecurityContext>
    {
        public abstract class PermissionBase<TInput> : IPermission<TInput>
        {
            public SecurityPolicy<TUserSecurityContext> Policy { get; }
            public string Name { get; protected set; }
            ISecurityPolicy FluentAuthorization.IPermission.Policy => Policy;
            
            public PermissionBase(SecurityPolicy<TUserSecurityContext> policy)
            {
                Policy = policy;
                Name = this.GetType().GetCustomAttribute<PermissionNameAttribute>()?.Name ?? this.GetType().Name;
            }

            public virtual string GetDenialMessage(TUserSecurityContext user, TInput input)
            {
                return "Permission " + Name + " of Policy " + Name + " was denied";
            }

            public abstract AssertionResult Assert(TUserSecurityContext user, TInput input);
        }
    }

    public partial class SecurityPolicy<T, TUserSecurityContext>
    {
        public abstract new class PermissionBase<TInput> : SecurityPolicy<TUserSecurityContext>.PermissionBase<TInput>, IPermission<TInput>
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