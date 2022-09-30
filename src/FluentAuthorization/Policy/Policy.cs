using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentAuthorization
{
    /// <summary>
    /// Abstract security policy. Use this as a base class for your security policy implementations.
    /// </summary>
    /// <typeparam name="TUser">The user context type.</typeparam>
    /// <typeparam name="TResource">The resource type.</typeparam>
    /// <typeparam name="TData">The policy data type.</typeparam>
    public abstract partial class Policy<TUser, TResource, TData> : IPolicy<TUser, TResource, TData>
    {
        Type IPolicy.ResourceType => typeof(TResource);
        Type IPolicy.DataType => typeof(TData);
        Type IPolicy.UserType => typeof(TUser);

        public virtual string Key => this.GetType().FullName;
        public virtual string Name => this.GetType().Name;
        public override string ToString() => Name;

        /// <summary>
        /// Aggregates a collection of policy data to single entry.
        /// </summary>
        /// <param name="data">The collection of data to aggregate.</param>
        /// <returns>A single instance of data that is the result of the aggregation.</returns>
        /// <exception cref="NotImplementedException">A not implemented exception is thrown by default of not overriden.</exception>
        public virtual TData Aggregate(IEnumerable<TData> data) => throw new NotImplementedException($"Data aggregation is not implemented for policy type {this.GetType().FullName}");

        protected readonly PermissionBuilder permissionBuilder = new();

        /// <summary>
        /// Controls whether data aggregation is performed before assertion. If true the assertion is perfomed on a single aggregated instance of the data. Default is false.
        /// </summary>
        protected bool AggregateDataBeforeAssertion { get; set; } = false;

        /// <summary>
        /// Controls whether an undefined(null) assertion result defaults to Deny. Default is true.
        /// </summary>
        protected bool TreatUndefinedAsDeny { get; set;  } = true;

        private static string BuildDefaultMessage(AssertionContextBase context)
        {
            return DefaultMessageBuilder.BuildMessage(context.User.ToString(), context.PolicyName, context.PermissionName);
        }

        /// <summary>
        /// Aggregates all returned assertions to a single result. The default implementations will assume Deny wins and aggregates all results (applies logical & on all results).
        /// </summary>
        /// <param name="results">All the results produced by the assertion.</param>
        /// <returns>An aggregated result.</returns>
        protected virtual AssertionResult AggregateAssertions(IEnumerable<AssertionResult> results)
        {
            return results.Aggregate(default(AssertionResult), (acc, el) => acc & el);
        }

        /// <summary>
        /// Overrides default assertion logic.
        /// </summary>
        /// <param name="user">The assertion context user.</param>
        /// <param name="resource">The assertion context resource.</param>
        /// <param name="permissionName">The name of the asserted permission.</param>
        /// <param name="data">The assertion context data.</param>
        /// <returns>The overriden assertion result or null to apply default assertion logic.</returns>
        protected virtual AssertionResult OverrideAssertion(TUser user, TResource resource, string permissionName, IEnumerable<TData> data)
        {
            return null;
        }

        internal AssertionResult Assert(TUser user, TResource resource, IPermission permission, IEnumerable<TData> data)
        {
            data = AggregateDataInternal(data);

            var overridenResult = OverrideAssertion(user, resource, permission.Name, data);
            if (overridenResult is not null) return overridenResult;

            var results = data.Select(d => permission.Assert(new AssertionContext(user, resource, d, permission, Name))).ToList();

            return AggregateAssertionsInternal(user.ToString(), permission.Name, results);
        }

        internal AssertionResult Assert<TState>(TUser user, TResource resource, IPermission<TState> permission, IEnumerable<TData> data, TState state)
        {
            data = AggregateDataInternal(data);

            var overridenResult = OverrideAssertion(user, resource, permission.Name, data);
            if (overridenResult is not null) return overridenResult;

            var results = data.Select(d => permission.Assert(new AssertionContext<TState>(user, resource, d, state, permission, Name))).ToList();

            return AggregateAssertionsInternal(user.ToString(), permission.Name, results);
        }

        private IEnumerable<TData> AggregateDataInternal(IEnumerable<TData> data)   
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
