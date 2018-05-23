using System;

namespace FluentAuthorization
{
    public partial class SecurityPolicy<TUserSecurityContext>
    {
        public class LazyPermission : IPermission
        {
            protected readonly Lazy<IPermission> Permission;
            public string Name => Permission.Value.Name;
            public SecurityPolicy<TUserSecurityContext> Policy { get; }
            ISecurityPolicy FluentAuthorization.IPermission.Policy => Policy;
            
            public LazyPermission(
                SecurityPolicy<TUserSecurityContext> policy,
                Func<IPermission> constructor)
            {
                //TODO: get stuff from attribute
                Policy = policy;
                Permission = new Lazy<IPermission>(constructor);
            }

            public LazyPermission(
                SecurityPolicy<TUserSecurityContext> policy, string name,
                Func<TUserSecurityContext, bool> assertionFunc,
                Func<TUserSecurityContext, string> denialMessageBuilder = null)
            {
                Policy = policy;
                Permission = new Lazy<IPermission>(()
                    => new GenericPermission(Policy, name, assertionFunc, denialMessageBuilder));
            }

            public AssertionResult Assert(TUserSecurityContext user)
            {
                return Permission.Value.Assert(user);
            }
        }
    }


    public partial class SecurityPolicy<T, TUserSecurityContext>
    {
        public new class LazyPermission : IPermission
        {
            protected readonly Lazy<IPermission> Permission;
            public string Name => Permission.Value.Name;
            public SecurityPolicy<T, TUserSecurityContext> Policy { get; }
            ISecurityPolicy FluentAuthorization.IPermission.Policy => Policy;
            SecurityPolicy<T, TUserSecurityContext> IPermission.Policy => Policy;
            SecurityPolicy<TUserSecurityContext> SecurityPolicy<TUserSecurityContext>.IPermission.Policy => Policy;

            public LazyPermission(
                SecurityPolicy<T, TUserSecurityContext> policy,
                Func<IPermission> constructor)
            {
                //TODO: get stuff from attribute
                Policy = policy;
                Permission = new Lazy<IPermission>(constructor);
            }

            public LazyPermission(
                SecurityPolicy<T, TUserSecurityContext> policy, string name,
                Func<T, TUserSecurityContext, bool> assertionFunc,
                Func<T, TUserSecurityContext, string> denialMessageBuilder = null)
            {
                Policy = policy;
                Permission = new Lazy<IPermission>(()
                    => new GenericPermission(Policy, name, assertionFunc, denialMessageBuilder));
            }

            public AssertionResult Assert(TUserSecurityContext user)
            {
                return Permission.Value.Assert(user);
            }
        }
    }

}