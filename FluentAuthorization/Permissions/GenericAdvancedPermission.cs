using System;

namespace FluentAuthorization
{
    public partial class SecurityPolicy<TUserSecurityContext>
    {
        public class GenericAdvancedPermission : PermissionBase
        {
            readonly Func<TUserSecurityContext, string> denialMessageBuilder;
            readonly Func<TUserSecurityContext, PermissionResult> assertionFunc;
            public GenericAdvancedPermission(SecurityPolicy<TUserSecurityContext> policy,
                string name,
                Func<TUserSecurityContext, PermissionResult> assertionFunc,
                Func<TUserSecurityContext, string> denialMessageBuilder = null)
                : base(policy)
            {
                Name = name;
                this.assertionFunc = assertionFunc;
                this.denialMessageBuilder = denialMessageBuilder;
            }

            public override AssertionResult Assert(TUserSecurityContext user)
            {
                return new AssertionResult(
                    new EffectivePermission(this, Policy, assertionFunc(user)));
            }

            public override string GetDenialMessage(TUserSecurityContext user)
            {
                return denialMessageBuilder == null ?
                    denialMessageBuilder(user) :
                    base.GetDenialMessage(user);
            }
        }
    }

    public partial class SecurityPolicy<T, TUserSecurityContext>
    {
        public new class GenericAdvancedPermission : PermissionBase
        {
            readonly Func<T, TUserSecurityContext, string> denialMessageBuilder;
            readonly Func<T, TUserSecurityContext, PermissionResult> assertionFunc;
            public GenericAdvancedPermission(SecurityPolicy<T, TUserSecurityContext> policy,
                string name,
                Func<T, TUserSecurityContext, PermissionResult> assertionFunc,
                Func<T, TUserSecurityContext, string> denialMessageBuilder = null)
                : base(policy)
            {
                Name = name;
                this.assertionFunc = assertionFunc;
                this.denialMessageBuilder = denialMessageBuilder;
            }

            public override AssertionResult Assert(TUserSecurityContext user)
            {
                return new AssertionResult(
                    new EffectivePermission(this, Policy, assertionFunc(Policy.Data, user)));
            }

            public override string GetDenialMessage(TUserSecurityContext user)
            {
                return denialMessageBuilder == null ?
                    denialMessageBuilder(Policy.Data, user) :
                    base.GetDenialMessage(user);
            }
        }
    }
}
