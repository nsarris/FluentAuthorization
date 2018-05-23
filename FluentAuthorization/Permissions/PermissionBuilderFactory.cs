using System;
using System.Linq.Expressions;
using System.Reflection;

namespace FluentAuthorization
{

    public partial class SecurityPolicy<TUserSecurityContext>
    {
        public class PermissionBuilderFactory<TPolicy>
            where TPolicy : SecurityPolicy<TUserSecurityContext>
        {
            readonly TPolicy policy;
            public PermissionBuilderFactory(TPolicy policy)
            {
                this.policy = policy;
            }
            public PermissionBuilder For(Expression<Func<TPolicy, IPermission>> PermissionSelector)
            {
                return new PermissionBuilder(policy, ReflectionHelper.GetProperty(PermissionSelector));
            }

            public PermissionBuilder For<TInput>(Expression<Func<TPolicy, IPermission<TInput>>> PermissionSelector)
            {
                return new PermissionBuilder(policy, ReflectionHelper.GetProperty(PermissionSelector));
            }

            
        }
    }

    public partial class SecurityPolicy<T, TUserSecurityContext>
    {
        public new class PermissionBuilderFactory<TPolicy>
            where TPolicy : SecurityPolicy<T, TUserSecurityContext>
        {
            readonly TPolicy policy;
            public PermissionBuilderFactory(TPolicy policy)
            {
                this.policy = policy;
            }
            public PermissionBuilder For(Expression<Func<TPolicy, IPermission>> PermissionSelector)
            {
                return new PermissionBuilder(policy, ReflectionHelper.GetProperty(PermissionSelector));
            }

            public PermissionBuilder For<TInput>(Expression<Func<TPolicy, IPermission<TInput>>> PermissionSelector)
            {
                return new PermissionBuilder(policy, ReflectionHelper.GetProperty(PermissionSelector));
            }
        }
    }
}
