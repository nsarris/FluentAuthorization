using System.Text;
using System.Threading.Tasks;

namespace FluentAuthorization
{
    public partial class SecurityPolicy<TUserSecurityContext>
    {
        public abstract class Permission : PermissionBase
        {
            public Permission(SecurityPolicy<TUserSecurityContext> policy) : base(policy)
            {
            }

            protected abstract bool AssertInternal(TUserSecurityContext user);

            public override AssertionResult Assert(TUserSecurityContext user)
            {
                var success = AssertInternal(user);
                var r = new EffectivePermission()
                {
                    Allow = success,
                    Policy = Policy,
                    Permission = this,
                    DenialMessage = success ? null : GetDenialMessage(user)
                };
                return new AssertionResult(r);
            }
        }
    }

    public partial class SecurityPolicy<T, TUserSecurityContext>
    {
        public abstract new class Permission : PermissionBase
        {
            public Permission(SecurityPolicy<T, TUserSecurityContext> policy) : base(policy)
            {
            }

            protected abstract bool AssertInternal(T data, TUserSecurityContext user);

            public override AssertionResult Assert(TUserSecurityContext user)
            {
                var success = AssertInternal(Policy.Data, user);
                var r = new EffectivePermission()
                {
                    Allow = success,
                    Policy = Policy,
                    Permission = this,
                    DenialMessage = success ? null : GetDenialMessage(user)
                };
                return new AssertionResult(r);
            }
        }
    }
}