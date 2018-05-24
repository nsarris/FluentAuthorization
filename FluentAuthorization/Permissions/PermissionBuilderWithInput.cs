using System;
using System.Reflection;

namespace FluentAuthorization
{
    public partial class SecurityPolicy<TUserSecurityContext>
    {
        public sealed class PermissionBuilder<TInput>
        {
            string name;
            Func<TUserSecurityContext, TInput, bool> assertionFunc;
            Func<TUserSecurityContext, TInput, PermissionResult> advancedAssertionFunc;
            Func<TUserSecurityContext, TInput, string> messageBuilder;
            
            SecurityPolicy<TUserSecurityContext> policy;
            PropertyInfo permissionProperty;

            internal PermissionBuilder(SecurityPolicy<TUserSecurityContext> policy, PropertyInfo PermissionProperty)
            {
                this.policy = policy;
                this.permissionProperty = PermissionProperty;
                this.name = PermissionProperty.GetCustomAttribute<PermissionNameAttribute>()?.Name
                    ?? PermissionProperty.Name;
                var message = PermissionProperty.GetCustomAttribute<PermissionDenialMessageAttribute>()?.Message;
                if (!string.IsNullOrEmpty(message))
                    messageBuilder = (TUserSecurityContext user, TInput _) => message;
            }

            public PermissionBuilder<TInput> WithName(string name)
            {
                this.name = name;
                return this;
            }

            public PermissionBuilder<TInput> Assert(Func<TUserSecurityContext, TInput, bool> assertionFunc)
            {
                this.assertionFunc = assertionFunc;
                return this;
            }

            public PermissionBuilder<TInput> Assert(Func<TUserSecurityContext, TInput, PermissionResult> assertionFunc)
            {
                this.advancedAssertionFunc = assertionFunc;
                return this;
            }

            public PermissionBuilder<TInput> WithMessageBuilder(Func<TUserSecurityContext, TInput, string> messageBuilder)
            {
                this.messageBuilder = messageBuilder;
                return this;
            }

            public IPermission<TInput> Build()
            {
                if (assertionFunc == null && advancedAssertionFunc == null)
                    throw new Exception("Assertion function not set, cannot build Permission");

                if (assertionFunc != null)
                    return new GenericPermission<TInput>(policy, name, assertionFunc, messageBuilder);
                else
                    return new GenericAdvancedPermission<TInput>(policy, name, advancedAssertionFunc, messageBuilder);
            }

            public IPermission<TInput> BuildAndSet()
            {
                if (!permissionProperty.CanWrite)
                    throw new InvalidOperationException("Cannot set Permission property " + permissionProperty + " of policy type " + policy.GetType().Name);

                var permission = Build();

                //PermissionProperty.SetValue(policy, Permission);
                ReflectionHelper.SetValue(permissionProperty, policy, permission);

                return permission;
            }

            public IPermission<TInput> BuildAndSetLazy(Func<PermissionBuilder<TInput>, PermissionBuilder<TInput>> lazyBuilder)
            {
                if (!permissionProperty.CanWrite)
                    throw new InvalidOperationException("Cannot set Permission property " + permissionProperty + " of policy type " + policy.GetType().Name);

                var permission = new LazyPermission<TInput>(policy, () => lazyBuilder(this).BuildAndSet());

                //permissionProperty.SetValue(policy, Permission);
                ReflectionHelper.SetValue(permissionProperty, policy, permission);

                return permission;
            }


        }
    }

    public partial class SecurityPolicy<T, TUserSecurityContext>
    {
        public new sealed class PermissionBuilder<TInput>
        {
            string name;
            Func<T, TUserSecurityContext,TInput, bool> assertionFunc;
            Func<T, TUserSecurityContext, TInput, PermissionResult> advancedAssertionFunc;
            Func<T, TUserSecurityContext, TInput, string> messageBuilder;

            SecurityPolicy<T, TUserSecurityContext> policy;
            PropertyInfo permissionProperty;

            internal PermissionBuilder(SecurityPolicy<T, TUserSecurityContext> policy, PropertyInfo PermissionProperty)
            {
                this.policy = policy;
                this.permissionProperty = PermissionProperty;
                this.name = PermissionProperty.GetCustomAttribute<PermissionNameAttribute>()?.Name
                    ?? PermissionProperty.Name;
                var message = PermissionProperty.GetCustomAttribute<PermissionDenialMessageAttribute>()?.Message;
                if (!string.IsNullOrEmpty(message))
                    messageBuilder = (T data, TUserSecurityContext user, TInput _) => message;
            }

            public PermissionBuilder<TInput> WithName(string name)
            {
                this.name = name;
                return this;
            }

            public PermissionBuilder<TInput> Assert(Func<T, TUserSecurityContext, TInput, bool> assertionFunc)
            {
                this.assertionFunc = assertionFunc;
                return this;
            }

            public PermissionBuilder<TInput> Assert(Func<T, TUserSecurityContext, TInput, PermissionResult> assertionFunc)
            {
                this.advancedAssertionFunc = assertionFunc;
                return this;
            }

            public PermissionBuilder<TInput> WithMessageBuilder(Func<T, TUserSecurityContext, TInput, string> messageBuilder)
            {
                this.messageBuilder = messageBuilder;
                return this;
            }

            public IPermission<TInput> Build()
            {
                if (assertionFunc == null && advancedAssertionFunc == null)
                    throw new Exception("Assertion function not set, cannot build Permission");

                if (assertionFunc != null)
                    return new GenericPermission<TInput>(policy, name, assertionFunc, messageBuilder);
                else
                    return new GenericAdvancedPermission<TInput>(policy, name, advancedAssertionFunc, messageBuilder);
            }

            public IPermission<TInput> BuildAndSet()
            {
                if (!permissionProperty.CanWrite)
                    throw new InvalidOperationException("Cannot set Permission property " + permissionProperty + " of policy type " + policy.GetType().Name);

                var permission = Build();

                //permissionProperty.SetValue(policy, Permission);
                ReflectionHelper.SetValue(permissionProperty, policy, permission);

                return permission;
            }

            public IPermission<TInput> BuildAndSetLazy(Func<PermissionBuilder<TInput>, PermissionBuilder<TInput>> lazyBuilder)
            {
                if (!permissionProperty.CanWrite)
                    throw new InvalidOperationException("Cannot set Permission property " + permissionProperty + " of policy type " + policy.GetType().Name);

                var permission = new LazyPermission<TInput>(policy, () => lazyBuilder(this).BuildAndSet());

                //permissionProperty.SetValue(policy, Permission);
                ReflectionHelper.SetValue(permissionProperty, policy, permission);

                return permission;
            }


        }
    }
}
