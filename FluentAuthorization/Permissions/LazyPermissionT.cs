using System;

namespace FluentAuthorization
{

    public partial class SecurityPolicy<TUserSecurityContext>
    {
        public class LazyPermission<TInput> : IPermission<TInput>
        {
            public SecurityPolicy<TUserSecurityContext> Policy { get; }
            readonly Lazy<IPermission<TInput>> Permission;
            public string Name => Permission.Value.Name;
            ISecurityPolicy FluentAuthorization.IPermission.Policy => Policy;

            public LazyPermission(SecurityPolicy<TUserSecurityContext> policy,
                Func<IPermission<TInput>> constructor)
            {
                //TODO: get stuff from attribute
                Policy = policy;
                Permission = new Lazy<IPermission<TInput>>(constructor);
            }

            public LazyPermission(SecurityPolicy<TUserSecurityContext> policy, string name,
                Func<TUserSecurityContext, TInput, bool> assertionFunc,
                Func<TUserSecurityContext, TInput, string> denialMessageBuilder = null)
            {
                Policy = policy;
                Permission = new Lazy<IPermission<TInput>>(()
                    => new GenericPermission<TInput>(Policy, name, assertionFunc, denialMessageBuilder));
            }

            public AssertionResult Assert(TUserSecurityContext user, TInput input)
            {
                return Permission.Value.Assert(user, input);
            }
        }
    }

    public partial class SecurityPolicy<T, TUserSecurityContext>
        where T : PolicyData
    {
        public new class LazyPermission<TInput> : IPermission<TInput>
        {
            public SecurityPolicy<T, TUserSecurityContext> Policy { get; }
            readonly Lazy<IPermission<TInput>> Permission;
            public string Name => Permission.Value.Name;
            SecurityPolicy<TUserSecurityContext> SecurityPolicy<TUserSecurityContext>.IPermission<TInput>.Policy => Policy;
            ISecurityPolicy FluentAuthorization.IPermission.Policy => Policy;

            public LazyPermission(SecurityPolicy<T,TUserSecurityContext> policy,
                Func<IPermission<TInput>> constructor)
            {
                //TODO: get stuff from attribute
                Policy = policy;
                Permission = new Lazy<IPermission<TInput>>(constructor);
            }

            public LazyPermission(SecurityPolicy<T, TUserSecurityContext> policy, string name,
                Func<T, TUserSecurityContext, TInput, bool> assertionFunc,
                Func<T, TUserSecurityContext, TInput, string> denialMessageBuilder = null)
            {
                Policy = policy;
                Permission = new Lazy<IPermission<TInput>>(()
                    => new GenericPermission<TInput>(Policy, name, assertionFunc, denialMessageBuilder));
            }

            public AssertionResult Assert(TUserSecurityContext user, TInput input)
            {
                return Permission.Value.Assert(user, input);
            }
        }
    }

}