using System;

namespace FluentAuthorization
{
    public abstract class Policy<TUser, TResource, T> : IPolicy<TResource, T>
    {
        Type IPolicy.ResourceType => typeof(TResource);
        Type IPolicy.DataType => typeof(T);
        Type IPolicy.UserType => typeof(TUser);

        public virtual string Key => this.GetType().FullName;
        public virtual string Name => this.GetType().Name;

        public override string ToString() => Name;
        
        public abstract class Permission : IPermission
        {
            public abstract AssertionResult Assert(AssertionContext context);
            public abstract string BuildMessage(AssertionContext context);
            public abstract string Name { get; }

            public override string ToString() => Name;
        }

        public abstract class Permission<TState> : IPermission<TState>
        {
            public abstract AssertionResult Assert(AssertionContext<TState> context);
            public abstract string BuildMessage(AssertionContext<TState> context);
            public abstract string Name { get; }

            public override string ToString() => Name;
        }

        private static class DefaultMessageBuilder
        {
            public static string BuildMessage(AssertionContextBase context)
            {
                return FluentAuthorization.DefaultMessageBuilder.BuildMessage(context.User.ToString(), context.PolicyName, context.PermissionName);
            }
        }

        public class DelegatePermission : Permission
        {
            private readonly Func<AssertionContext, AssertionResult> assert;
            private readonly Func<AssertionContext, string> messageBuilder;
            
            public DelegatePermission(
                Func<AssertionContext, AssertionResult> assert,
                string name,
                Func<AssertionContext, string> messageBuilder)
            {
                this.assert = assert;
                this.messageBuilder = messageBuilder ?? DefaultMessageBuilder.BuildMessage;
                Name = name;
            }

            public override string Name { get; }

            public override AssertionResult Assert(AssertionContext context)
                => assert(context);

            public override string BuildMessage(AssertionContext context)
                => messageBuilder(context);
        }

        public class DelegatePermission<TState> : Permission<TState>
        {
            private readonly Func<AssertionContext<TState>, AssertionResult> assert;
            private readonly Func<AssertionContext<TState>, string> messageBuilder;

            public override string Name { get; }

            public DelegatePermission(
                Func<AssertionContext<TState>, AssertionResult> assert,
                string name,
                Func<AssertionContext<TState>, string> messageBuilder)
            {
                this.assert = assert;
                this.messageBuilder = messageBuilder ?? DefaultMessageBuilder.BuildMessage;
                Name = name;
            }

            public override AssertionResult Assert(AssertionContext<TState> context)
                => assert(context);

            public override string BuildMessage(AssertionContext<TState> context)
                => messageBuilder(context);
        }

        public abstract class AssertionContextBase
        {
            protected AssertionContextBase(TUser user, TResource resource, T data, string permissionName, string policyName)
            {
                Data = data;
                Resource = resource;
                User = user;
                PermissionName = permissionName;
                PolicyName = policyName;
            }

            public TUser User { get; set; }
            public TResource Resource { get; }
            public T Data { get; }
            public string PermissionName { get; }
            public string PolicyName { get; }

            public AssertionResult Allow() => AssertionResult.Success;
            //public AssertionResult Deny(string reason = null) => new(new AssertionFailure(PermissionName, PolicyName, reason));
        }

        public class AssertionContext : AssertionContextBase
        {
            private readonly Permission permission;

            internal AssertionContext(TUser user, TResource resource, T data, Permission permission, string policyName) 
                : base(user, resource, data, permission.Name, policyName)
            {
                this.permission = permission;
            }

            public AssertionResult Deny(string reason = null) => new(new AssertionFailure(User.ToString(), PermissionName, PolicyName, permission.BuildMessage(this), reason));
        }

        public class AssertionContext<TState> : AssertionContextBase
        {
            private readonly Permission<TState> permission;

            internal AssertionContext(TUser user, TResource resource, T data, TState state, Permission<TState> permission, string policyName)
                : base(user, resource, data, permission.Name, policyName)
            {
                State = state;
                this.permission = permission;
            }

            public TState State { get; }

            public AssertionResult Deny(string reason = null) => new(new AssertionFailure(User.ToString(), PermissionName, PolicyName, permission.BuildMessage(this), reason));
        }

        public class PolicyBuilderRoot
        {
            public PolicyBuilder AssertWith(Func<AssertionContext, AssertionResult> assert)
            {
                return new PolicyBuilder(assert);
            }

            public PolicyBuilder AssertWith(Func<AssertionContext, bool> assert)
            {
                return new PolicyBuilder(ctx => assert(ctx) ? ctx.Allow() : ctx.Deny());
            }

            public PolicyBuilder<TState> AssertWith<TState>(Func<AssertionContext<TState>, AssertionResult> assert)
            {
                return new PolicyBuilder<TState>(assert);
            }

            public PolicyBuilder<TState> AssertWith<TState>(Func<AssertionContext<TState>, bool> assert)
            {
                return new PolicyBuilder<TState>(ctx => assert(ctx) ? ctx.Allow() : ctx.Deny());
            }
        }

        public class PolicyBuilder
        {
            private readonly Func<AssertionContext, AssertionResult> assert;
            private Func<AssertionContext, string> messageBuilder;
            private string name;

            public PolicyBuilder(Func<AssertionContext, AssertionResult> assert)
            {
                this.assert = assert;
            }

            public PolicyBuilder WithName(string name)
            {
                this.name = name;
                return this;
            }

            public PolicyBuilder WithMessage(Func<AssertionContext, string> messageBuilder)
            {
                this.messageBuilder = messageBuilder;
                return this;
            }

            public Permission Build()
                => new DelegatePermission(assert, name, messageBuilder);
        }

        public class PolicyBuilder<TState>
        {
            private readonly Func<AssertionContext<TState>, AssertionResult> assert;
            private Func<AssertionContext<TState>, string> messageBuilder;
            private string name;

            public PolicyBuilder(Func<AssertionContext<TState>, AssertionResult> assert)
            {
                this.assert = assert;
            }

            public PolicyBuilder<TState> WithName(string name)
            {
                this.name = name;
                return this;
            }

            public PolicyBuilder<TState> WithMessage(Func<AssertionContext<TState>, string> messageBuilder)
            {
                this.messageBuilder = messageBuilder;
                return this;
            }

            public Permission<TState> Build()
                => new DelegatePermission<TState>(assert, name, messageBuilder);
        }

        protected readonly PolicyBuilderRoot policyBuilder = new();

        protected Policy()
        {

        }
    }
}
