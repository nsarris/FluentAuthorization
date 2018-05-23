using System;

namespace FluentAuthorization
{

    public partial class SecurityPolicy<TUserSecurityContext>
    {
        public class GenericPermission : PermissionBase
        {
            readonly Func<TUserSecurityContext, string> denialMessageBuilder;
            readonly Func<TUserSecurityContext, bool> assertionFunc;

            public GenericPermission(SecurityPolicy<TUserSecurityContext> policy,
                string name,
                Func<TUserSecurityContext, bool> assertionFunc,
                Func<TUserSecurityContext, string> denialMessageBuilder = null)
                : base(policy)
            {
                Name = name;
                this.assertionFunc = assertionFunc;
                this.denialMessageBuilder = denialMessageBuilder;
            }

            public override AssertionResult Assert(TUserSecurityContext user)
            {
                var success = assertionFunc(user);
                var r = new EffectivePermission()
                {
                    Allow = success,
                    Policy = Policy,
                    Permission = this,
                    DenialMessage = success ? null : GetDenialMessage(user)
                };
                return new AssertionResult(r);
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
        public new class GenericPermission : PermissionBase
        {
            readonly Func<T, TUserSecurityContext, string> denialMessageBuilder;
            readonly Func<T, TUserSecurityContext, bool> assertionFunc;

            public GenericPermission(SecurityPolicy<T, TUserSecurityContext> policy,
                string name,
                Func<T, TUserSecurityContext, bool> assertionFunc,
                Func<T, TUserSecurityContext, string> denialMessageBuilder = null)
                : base(policy)
            {
                Name = name;
                this.assertionFunc = assertionFunc;
                this.denialMessageBuilder = denialMessageBuilder;
            }

            public override AssertionResult Assert(TUserSecurityContext user)
            {
                var success = assertionFunc(Policy.Data, user);
                var r = new EffectivePermission()
                {
                    Allow = success,
                    Policy = Policy,
                    Permission = this,
                    DenialMessage = success ? null : GetDenialMessage(user)
                };
                return new AssertionResult(r);
            }

            public override string GetDenialMessage(TUserSecurityContext user)
            {
                return denialMessageBuilder == null ?
                    denialMessageBuilder(Policy.Data , user) :
                    base.GetDenialMessage(user);
            }
        }
    }
}
