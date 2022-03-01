using System;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;

namespace FluentAuthorization
{
    public class PermissionCalculationMergePolicyStrategy : IPermissionCalculationStrategy
    {
        readonly IPolicyFactory policyFactory;
        public PermissionCalculationMergePolicyStrategy(IPolicyFactory policyFactory)
        {
            this.policyFactory = policyFactory;
        }

        public AssertionResult Calculate<T>(IEnumerable<T> policies, Func<T, AssertionResult> assertion)
            where T : ISecurityPolicy
        {
            if (typeof(ISecurityPolicyWithData).IsAssignableFrom(typeof(T)))
            {
                var data = policies
                    .Cast<ISecurityPolicyWithData>()
                    .Select(x => x.Data)
                    .Aggregate((current, next) => current.Merge(next));

                var policy = policyFactory.Create(typeof(T), data);

                if (policy == null)
                    throw new InvalidOperationException($"Null policy returned from policy factory for policy type {typeof(T).Name}");

                if (policy.GetType() != typeof(T))
                    throw new InvalidOperationException($"Policy factory for policy type {typeof(T).Name} returned a different type ({policy.GetType().Name}");

                return assertion((T)policy);
            }
            else
            {
                return assertion(
                    policies
                        .Aggregate((x, next) => (T)x.Merge(next)));
            }
        }
    }

}
