using System;

namespace FluentAuthorization
{
    public partial class SecurityPolicy<TUserSecurityContext>
    {
        public class GenericAdvancedPermission<TInput> : PermissionBase<TInput>
        {
            readonly Func<TUserSecurityContext, TInput, string> denialMessageBuilder;
            readonly Func<TUserSecurityContext, TInput, PermissionResult> assertionFunc;
            public GenericAdvancedPermission(SecurityPolicy<TUserSecurityContext> policy,
                string name,
                Func<TUserSecurityContext, TInput, PermissionResult> assertionFunc,
                Func<TUserSecurityContext, TInput, string> denialMessageBuilder = null)
                : base(policy)
            {
                Name = name;
                this.assertionFunc = assertionFunc;
                this.denialMessageBuilder = denialMessageBuilder;
            }

            public override AssertionResult Assert(TUserSecurityContext user, TInput input)
            {
                return new AssertionResult(
                    new EffectivePermission(this, Policy, assertionFunc(user, input)));
            }

            public override string GetDenialMessage(TUserSecurityContext user, TInput input)
            {
                return denialMessageBuilder == null ?
                    denialMessageBuilder(user, input) :
                    base.GetDenialMessage(user, input);
            }
        }
    }

    public partial class SecurityPolicy<T, TUserSecurityContext>
    {
        public new class GenericAdvancedPermission<TInput> : PermissionBase<TInput>
        {
            readonly Func<T, TUserSecurityContext, TInput, string> denialMessageBuilder;
            readonly Func<T, TUserSecurityContext, TInput, PermissionResult> assertionFunc;
            public GenericAdvancedPermission(SecurityPolicy<T, TUserSecurityContext> policy,
                string name,
                Func<T, TUserSecurityContext, TInput, PermissionResult> assertionFunc,
                Func<T, TUserSecurityContext, TInput, string> denialMessageBuilder = null)
                : base(policy)
            {
                Name = name;
                this.assertionFunc = assertionFunc;
                this.denialMessageBuilder = denialMessageBuilder;
            }

            public override AssertionResult Assert(TUserSecurityContext user, TInput input)
            {
                return new AssertionResult(
                    new EffectivePermission(this, Policy, assertionFunc(Policy.Data, user, input)));
            }

            public override string GetDenialMessage(TUserSecurityContext user, TInput input)
            {
                return denialMessageBuilder == null ?
                    denialMessageBuilder(Policy.Data, user, input) :
                    base.GetDenialMessage(user, input);
            }
        }
    }
}
