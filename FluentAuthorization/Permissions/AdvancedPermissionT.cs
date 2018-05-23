namespace FluentAuthorization
{
    public partial class SecurityPolicy<TUserSecurityContext>
    {
        public abstract class AdvancedPermission<TInput> : PermissionBase<TInput>
        {
            public AdvancedPermission(SecurityPolicy<TUserSecurityContext> policy) : base(policy)
            {
            }

            protected abstract PermissionResult AssertInternal(TUserSecurityContext user, TInput input);

            public override AssertionResult Assert(TUserSecurityContext user, TInput input)
            {
                var r = new EffectivePermission(this, Policy, AssertInternal(user, input));
                return new AssertionResult(r);
            }
        }
    }

    public partial class SecurityPolicy<T, TUserSecurityContext>
    {
        public abstract new class AdvancedPermission<TInput> : PermissionBase<TInput>
        {
            public AdvancedPermission(SecurityPolicy<T, TUserSecurityContext> policy) : base(policy)
            {
            }

            protected abstract PermissionResult AssertInternal(T data, TUserSecurityContext user, TInput input);

            public override AssertionResult Assert(TUserSecurityContext user, TInput input)
            {
                var r = new EffectivePermission(this, Policy, AssertInternal(Policy.Data, user, input));
                return new AssertionResult(r);
            }
        }
    }
}
