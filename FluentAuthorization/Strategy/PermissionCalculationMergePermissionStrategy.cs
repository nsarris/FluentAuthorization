using System;
using System.Linq;
using System.Collections.Generic;

namespace FluentAuthorization
{
    public class PermissionCalculationMergePermissionStrategy : IPermissionCalculationStrategy
    {
        readonly IPermissionReduceStrategy mergeStrategy;

        public PermissionCalculationMergePermissionStrategy()
        {
            mergeStrategy = PermissionMergeDenyWinsStrategy.Instance;
        }

        public PermissionCalculationMergePermissionStrategy(IPermissionReduceStrategy mergeStrategy)
        {
            this.mergeStrategy = mergeStrategy;
        }
        
        public AssertionResult Calculate<T>(IEnumerable<T> policies, Func<T, AssertionResult> Permission)
            where T : ISecurityPolicy
        {
            return mergeStrategy.Reduce(policies.Select(x => Permission(x)));
        }
    }

}
