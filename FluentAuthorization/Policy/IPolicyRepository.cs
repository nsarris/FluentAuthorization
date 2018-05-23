using System;
using System.Collections.Generic;

namespace FluentAuthorization
{
    public interface IPolicyRepository<T>
    {
        List<SecurityPolicy<T>> GetByUserSecurityContext(T securityContext);
        List<SecurityPolicy<T>> GetByUserSecurityContext(Type policyType, T securityContext);
        List<TPolicy> GetByUserSecurityContext<TPolicy>(T securityContext) where TPolicy : SecurityPolicy<T>;
    }
}
