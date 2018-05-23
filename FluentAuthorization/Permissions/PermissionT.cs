using System.Text;
using System.Threading.Tasks;

namespace FluentAuthorization
{
    public partial class SecurityPolicy<TUserSecurityContext>
    {
        public abstract class Permission<TInput> : PermissionBase<TInput>
        {
            public Permission(SecurityPolicy<TUserSecurityContext> policy) : base(policy)
            {
            }

            protected abstract bool AssertInternal(TUserSecurityContext user, TInput input);

            public override AssertionResult Assert(TUserSecurityContext user, TInput input)
            {
                var success = AssertInternal(user, input);
                var r = new EffectivePermission()
                {
                    Allow = success,
                    Policy = Policy,
                    Permission = this,
                    DenialMessage = success ? null : GetDenialMessage(user, input)
                };
                return new AssertionResult(r);
            }
        }
    }

    public partial class SecurityPolicy<T, TUserSecurityContext>
    {
        public abstract new class Permission<TInput> : PermissionBase<TInput>
        {
            public Permission(SecurityPolicy<T, TUserSecurityContext> policy) : base(policy)
            {
            }

            protected abstract bool AssertInternal(T data, TUserSecurityContext user, TInput input);
            
            public override AssertionResult Assert(TUserSecurityContext user, TInput input)
            {
                var success = AssertInternal(Policy.Data, user, input);
                var r = new EffectivePermission()
                {
                    Allow = success,
                    Policy = Policy,
                    Permission = this,
                    DenialMessage = success ? null : GetDenialMessage(user, input)
                };
                return new AssertionResult(r);
            }
        }
    }
}