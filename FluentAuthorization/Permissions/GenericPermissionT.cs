using System;

namespace FluentAuthorization
{

    public partial class SecurityPolicy<TUserSecurityContext>
    {
        public class GenericPermission<TInput> : PermissionBase<TInput>
        {
            readonly Func<TUserSecurityContext, TInput, string> denialMessageBuilder;
            readonly Func<TUserSecurityContext, TInput, bool> assertionFunc;

            public GenericPermission(SecurityPolicy<TUserSecurityContext> policy,
                string name,
                Func<TUserSecurityContext, TInput, bool> assertionFunc,
                Func<TUserSecurityContext, TInput, string> denialMessageBuilder = null)
                : base(policy)
            {
                Name = name;
                this.assertionFunc = assertionFunc;
                this.denialMessageBuilder = denialMessageBuilder;
            }

            public override AssertionResult Assert(TUserSecurityContext user, TInput input)
            {
                var success = assertionFunc(user, input);
                var r = new EffectivePermission()
                {
                    Allow = success,
                    Policy = Policy,
                    Permission = this,
                    DenialMessage = success ? null : GetDenialMessage(user, input)
                };
                return new AssertionResult(r);
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
        public new class GenericPermission<TInput> : PermissionBase<TInput>
        {
            readonly Func<T, TUserSecurityContext, TInput, string> denialMessageBuilder;
            readonly Func<T, TUserSecurityContext, TInput, bool> assertionFunc;

            public GenericPermission(SecurityPolicy<T, TUserSecurityContext> policy,
                string name,
                Func<T, TUserSecurityContext, TInput, bool> assertionFunc,
                Func<T, TUserSecurityContext, TInput, string> denialMessageBuilder = null)
                : base(policy)
            {
                Name = name;
                this.assertionFunc = assertionFunc;
                this.denialMessageBuilder = denialMessageBuilder;
            }

            public override AssertionResult Assert(TUserSecurityContext user, TInput input)
            {
                var success = assertionFunc(Policy.Data, user, input);
                var r = new EffectivePermission()
                {
                    Allow = success,
                    Policy = Policy,
                    Permission = this,
                    DenialMessage = success ? null : GetDenialMessage(user, input)
                };
                return new AssertionResult(r);
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
