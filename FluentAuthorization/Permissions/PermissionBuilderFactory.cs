using System;
using System.Linq.Expressions;
using System.Reflection;

namespace FluentAuthorization
{

    public partial class SecurityPolicy<TUserSecurityContext>
    {
        public static class PermissionBuilderFactory
        {
            public static PermissionBuilderFactory<TPolicy> Get<TPolicy>(TPolicy policy)
                where TPolicy : SecurityPolicy<TUserSecurityContext>
            {
                return new PermissionBuilderFactory<TPolicy>(policy);
            }
        }
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
        public new static class PermissionBuilderFactory
        {
            public static PermissionBuilderFactory<TPolicy> Get<TPolicy>(TPolicy policy)
                where TPolicy : SecurityPolicy<T, TUserSecurityContext>
            {
                return new PermissionBuilderFactory<TPolicy>(policy);
            }
        }
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

            public PermissionBuilder<TInput> For<TInput>(Expression<Func<TPolicy, IPermission<TInput>>> PermissionSelector)
            {
                return new PermissionBuilder<TInput>(policy, ReflectionHelper.GetProperty(PermissionSelector));
            }

            public static PermissionBuilderFactory<TPolicy> Get(TPolicy policy)
            {
                return new PermissionBuilderFactory<TPolicy>(policy);
            }
        }
    }
}
