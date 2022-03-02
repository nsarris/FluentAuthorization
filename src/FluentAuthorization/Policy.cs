using System;

namespace FluentAuthorization
{
    public abstract class Policy<TUser, TResource, T> : IPolicy<TResource, T>, IPolicyWithResource<TResource>
    {
        Type IPolicy.ResourceType => typeof(TResource);
        Type IPolicy.DataType => typeof(T);
        Type IPolicy.UserType => typeof(TUser);

        public virtual string Key => this.GetType().FullName;

        public abstract class Permission : IPermission
        {
            public abstract AssertionResult Assert(AssertionContext context);
        }

        public abstract class Permission<TState> : IPermission<TState>
        {
            public abstract AssertionResult Assert(AssertionContext<TState> context);
        }

        public class DelegatePermission : Permission
        {
            private readonly Func<AssertionContext, AssertionResult> assert;

            public DelegatePermission(Func<AssertionContext, AssertionResult> assert)
            {
                this.assert = assert;
            }

            public override AssertionResult Assert(AssertionContext context)
                => assert(context);
        }

        public class DelegatePermission<TState> : Permission<TState>
        {
            private readonly Func<AssertionContext<TState>, AssertionResult> assert;

            public DelegatePermission(Func<AssertionContext<TState>, AssertionResult> assert)
            {
                this.assert = assert;
            }

            public override AssertionResult Assert(AssertionContext<TState> context)
                => assert(context);
        }

        public class AssertionContext
        {
            public AssertionContext(TUser user, T data, IServiceProvider serviceProvider = null)
            {
                ServiceProvider = serviceProvider;
                Data = data;
                User = user;
            }

            public IServiceProvider ServiceProvider { get; }
            public TUser User { get; set; }
            public TResource Resource { get; }
            public T Data { get; }

            public AssertionResult Allow() => new AssertionResult(true);
            public AssertionResult Deny(string reason) => new AssertionResult(false, reason);
        }

        public class AssertionContext<TState> : AssertionContext
        {
            public AssertionContext(TUser user, T data, TState state, IServiceProvider serviceProvider = null)
                : base(user, data, serviceProvider)
            {
                State = state;
            }

            public TState State { get; }
        }

        public class PolicyBuilder
        {
            Func<AssertionContext, AssertionResult> assert;

            public PolicyBuilder AssertWith(Func<AssertionContext, AssertionResult> assert)
            {
                this.assert = assert;
                return this;
            }

            public PolicyBuilder AssertWith(Func<AssertionContext, bool> assert)
            {
                this.assert = ctx => new  AssertionResult(assert(ctx));
                return this;
            }

            public PolicyBuilder<TState> AssertWith<TState>(Func<AssertionContext<TState>, AssertionResult> assert)
            {
                return new PolicyBuilder<TState>(assert);
            }

            public PolicyBuilder<TState> AssertWith<TState>(Func<AssertionContext<TState>, bool> assert)
            {
                return new PolicyBuilder<TState>(ctx => new AssertionResult(assert(ctx)));
            }

            public Permission Build()
                => new DelegatePermission(assert);
        }

        public class PolicyBuilder<TState>
        {
            Func<AssertionContext<TState>, AssertionResult> assert;

            public PolicyBuilder(Func<AssertionContext<TState>, AssertionResult> assert)
            {
                this.assert = assert;
            }

            public Permission<TState> Build()
                => new DelegatePermission<TState>(assert);
        }

        protected readonly PolicyBuilder policyBuilder = new PolicyBuilder();

        public Policy()
        {

        }
    }
}
