using System;
using System.Collections.Generic;

namespace FluentAuthorization
{
    public interface IPermissionCalculationStrategy
    {
        AssertionResult Calculate<T>(IEnumerable<T> policies, Func<T, AssertionResult> assertion)
            where T : ISecurityPolicy;

            
    }

}
