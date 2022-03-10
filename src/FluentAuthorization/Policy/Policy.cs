using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentAuthorization
{
    public abstract partial class Policy<TUser, TResource, T> : IPolicy<TUser, TResource, T>
    {
        Type IPolicy.ResourceType => typeof(TResource);
        Type IPolicy.DataType => typeof(T);
        Type IPolicy.UserType => typeof(TUser);

        public virtual string Key => this.GetType().FullName;
        public virtual string Name => this.GetType().Name;
        public override string ToString() => Name;

        //public abstract T Aggregate(IEnumerable<T> data);
        public virtual T Aggregate(IEnumerable<T> data) => throw new NotImplementedException($"Data aggregation is not implemented for policy type {this.GetType().FullName}");

        protected readonly PermissionBuilder permissionBuilder = new();

        protected bool AggregateDataBeforeAssertion { get; } = false;
        protected bool TreatUndefinedAsDeny { get; } = true;

        private static string BuildDefaultMessage(AssertionContextBase context)
        {
            return DefaultMessageBuilder.BuildMessage(context.User.ToString(), context.PolicyName, context.PermissionName);
        }

        protected virtual AssertionResult AggregateAssertions(IEnumerable<AssertionResult> results)
        {
            return results.Aggregate(AssertionResult.Success, (acc, el) => acc && el);
        }

        internal AssertionResult Assert(TUser user, TResource resource, IPermission permission, IEnumerable<T> data)
        {
            data = AggregateDataInternal(data);

            var results = data.Select(d => permission.Assert(new AssertionContext(user, resource, d, permission, Name)));

            return AggregateAssertionsInternal(user.ToString(), permission.Name, results);
        }

        internal AssertionResult Assert<TState>(TUser user, TResource resource, IPermission<TState> permission, IEnumerable<T> data, TState state)
        {
            data = AggregateDataInternal(data);

            var results = data.Select(d => permission.Assert(new AssertionContext<TState>(user, resource, d, state, permission, Name)));

            return AggregateAssertionsInternal(user.ToString(), permission.Name, results);
        }

        private IEnumerable<T> AggregateDataInternal(IEnumerable<T> data)   
        {
            if (AggregateDataBeforeAssertion
                && data.Any())
            {
                data = new[] { Aggregate(data) };
            }

            return data;
        }

        private AssertionResult AggregateAssertionsInternal(string user, string permissionName, IEnumerable<AssertionResult> results)
        {
            if (results.Any())
            {
                var result = AggregateAssertions(results);
                if (result is not null)
                    return result;
            }

            if (TreatUndefinedAsDeny)
                return new AssertionResult(BuildUncofiguredPolicyFailure(user, permissionName));
            else
                return AssertionResult.Success;
        }

        private AssertionFailure BuildUncofiguredPolicyFailure(string user, string permissionName)
            => new(user, permissionName, Name, DefaultMessageBuilder.BuildMessage(user, permissionName, Name), "Policy not configured.");
    }
}
