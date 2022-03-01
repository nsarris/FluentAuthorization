using System;
using System.Reflection;

namespace FluentAuthorization
{
    public partial class SecurityPolicy<TUserSecurityContext>
    {
        public sealed class PermissionBuilder
        {
            private string name;
            private Func<TUserSecurityContext, bool> assertionFunc;
            private Func<TUserSecurityContext, PermissionResult> advancedAssertionFunc;
            private Func<TUserSecurityContext, string> messageBuilder;
            
            private readonly SecurityPolicy<TUserSecurityContext> policy;
            private readonly PropertyInfo permissionProperty;
            
            internal PermissionBuilder(SecurityPolicy<TUserSecurityContext> policy, PropertyInfo PermissionProperty)
            {
                this.policy = policy;
                this.permissionProperty = PermissionProperty;
                this.name = PermissionProperty.GetCustomAttribute<PermissionNameAttribute>()?.Name
                    ?? PermissionProperty.Name;
                var message = PermissionProperty.GetCustomAttribute<PermissionDenialMessageAttribute>()?.Message;
                if (!string.IsNullOrEmpty(message))
                    messageBuilder = (TUserSecurityContext user) => message;
            }

            public PermissionBuilder WithName(string name)
            {
                this.name = name;
                return this;
            }

            public PermissionBuilder Assert(Func<TUserSecurityContext, bool> assertionFunc)
            {
                this.assertionFunc = assertionFunc;
                return this;
            }

            public PermissionBuilder Assert(Func<TUserSecurityContext, PermissionResult> assertionFunc)
            {
                this.advancedAssertionFunc = assertionFunc;
                return this;
            }

            public PermissionBuilder WithMessageBuilder(Func<TUserSecurityContext, string> messageBuilder)
            {
                this.messageBuilder = messageBuilder;
                return this;
            }

            public IPermission Build()
            {
                if (assertionFunc == null && advancedAssertionFunc == null)
                    throw new InvalidOperationException("Assertion function not set, cannot build Permission");

                if (assertionFunc != null)
                    return new GenericPermission(policy, name, assertionFunc, messageBuilder);
                else
                    return new GenericAdvancedPermission(policy, name, advancedAssertionFunc, messageBuilder);
            }

            public IPermission BuildAndSet()
            {
                if (!permissionProperty.CanWrite)
                    throw new InvalidOperationException("Cannot set Permission property " + permissionProperty + " of policy type " + policy.GetType().Name);

                var permission = Build();

                ReflectionHelper.SetValue(permissionProperty, policy, permission);

                return permission;
            }

            public IPermission BuildAndSetLazy(Func<PermissionBuilder, PermissionBuilder> lazyBuilder)
            {
                if (!permissionProperty.CanWrite)
                    throw new InvalidOperationException("Cannot set Permission property " + permissionProperty + " of policy type " + policy.GetType().Name);

                var permission = new LazyPermission(policy, () => lazyBuilder(this).BuildAndSet());

                ReflectionHelper.SetValue(permissionProperty, policy, permission);

                return permission;
            }
        }
    }

    public partial class SecurityPolicy<T, TUserSecurityContext>
    {
        public new sealed class PermissionBuilder
        {
            private string name;
            private Func<T, TUserSecurityContext, bool> assertionFunc;
            private Func<T, TUserSecurityContext, PermissionResult> advancedAssertionFunc;
            private Func<T, TUserSecurityContext, string> messageBuilder;
            
            private readonly SecurityPolicy<T, TUserSecurityContext> policy;
            private readonly PropertyInfo permissionProperty;

            internal PermissionBuilder(SecurityPolicy<T, TUserSecurityContext> policy, PropertyInfo PermissionProperty)
            {
                this.policy = policy;
                this.permissionProperty = PermissionProperty;
                this.name = PermissionProperty.GetCustomAttribute<PermissionNameAttribute>()?.Name
                    ?? PermissionProperty.Name;
                var message = PermissionProperty.GetCustomAttribute<PermissionDenialMessageAttribute>()?.Message;
                if (!string.IsNullOrEmpty(message))
                    messageBuilder = (T data, TUserSecurityContext user) => message;
            }

            public PermissionBuilder WithName(string name)
            {
                this.name = name;
                return this;
            }

            public PermissionBuilder Assert(Func<T, TUserSecurityContext, bool> assertionFunc)
            {
                this.assertionFunc = assertionFunc;
                return this;
            }

            public PermissionBuilder Assert(Func<T, TUserSecurityContext, PermissionResult> assertionFunc)
            {
                this.advancedAssertionFunc = assertionFunc;
                return this;
            }

            public PermissionBuilder WithMessageBuilder(Func<T, TUserSecurityContext, string> messageBuilder)
            {
                this.messageBuilder = messageBuilder;
                return this;
            }

            public IPermission Build()
            {
                if (assertionFunc == null && advancedAssertionFunc == null)
                    throw new InvalidOperationException("Assertion function not set, cannot build Permission");

                if (assertionFunc != null)
                    return new GenericPermission(policy, name, assertionFunc, messageBuilder);
                else
                    return new GenericAdvancedPermission(policy, name, advancedAssertionFunc, messageBuilder);
            }

            public IPermission BuildAndSet()
            {
                if (!permissionProperty.CanWrite)
                    throw new InvalidOperationException("Cannot set Permission property " + permissionProperty + " of policy type " + policy.GetType().Name);

                var permission = Build();

                ReflectionHelper.SetValue(permissionProperty, policy, permission);

                return permission;
            }

            public IPermission BuildAndSetLazy(Func<PermissionBuilder, PermissionBuilder> lazyBuilder)
            {
                if (!permissionProperty.CanWrite)
                    throw new InvalidOperationException("Cannot set Permission property " + permissionProperty + " of policy type " + policy.GetType().Name);

                var permission = new LazyPermission(policy, () => lazyBuilder(this).BuildAndSet());

                ReflectionHelper.SetValue(permissionProperty, policy, permission);

                return permission;
            }
        }
    }
}
