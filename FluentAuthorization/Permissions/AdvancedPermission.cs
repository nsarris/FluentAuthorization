namespace FluentAuthorization
{
    public partial class SecurityPolicy<TUserSecurityContext>
    {
        public abstract class AdvancedPermission : PermissionBase
        {
            public AdvancedPermission(SecurityPolicy<TUserSecurityContext> policy) : base(policy)
            {
            }

            protected abstract PermissionResult AssertInternal(TUserSecurityContext user);

            public override AssertionResult Assert(TUserSecurityContext user)
            {
                var r = new EffectivePermission(this, Policy, AssertInternal(user));
                return new AssertionResult(r);
            }
        }
    }

    public partial class SecurityPolicy<T, TUserSecurityContext>
    {
        public abstract new class AdvancedPermission : PermissionBase
        {
            public AdvancedPermission(SecurityPolicy<T,TUserSecurityContext> policy) : base(policy)
            {
            }

            protected abstract PermissionResult AssertInternal(T data, TUserSecurityContext user);

            public override AssertionResult Assert(TUserSecurityContext user)
            {
                var r = new EffectivePermission(this, Policy, AssertInternal(Policy.Data , user));
                return new AssertionResult(r);
            }
        }
    }
}
