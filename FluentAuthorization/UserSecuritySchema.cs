using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentAuthorization
{
    public partial class UserSecuritySchema<T>
    {
        private readonly T userSecurityContext;
        private readonly IPolicyRepository<T> policyRepository;
        private readonly IPermissionCalculationStrategy calculationStrategy;
        
        public UserSecuritySchema(
            IPolicyRepository<T> policyRepository,
            T userSecurityContext)
        {
            this.policyRepository = policyRepository;
            this.userSecurityContext = userSecurityContext;
            calculationStrategy = new PermissionCalculationMergePermissionStrategy();
        }

        public UserSecuritySchema(
            IPolicyRepository<T> policyRepository,
            T securityContext,
            IPermissionCalculationStrategy calculationStrategy)
            :this(policyRepository, securityContext)
        {
            this.calculationStrategy = calculationStrategy;
        }

        public IEnumerable<TPolicy> GetPolicies<TPolicy>() 
            where TPolicy : SecurityPolicy<T>
        {
            return policyRepository.GetByUserSecurityContext(userSecurityContext).OfType<TPolicy>();
        }

        public AssertionResult Assert<TPolicy>(Func<TPolicy, SecurityPolicy<T>.IPermission> permissionSelector)
            where TPolicy : SecurityPolicy<T>
        {
            return WhenAll(a => a.Has(permissionSelector)).Assert();
        }

        public AssertionResult Assert<TPolicy, TInput>(Func<TPolicy, SecurityPolicy<T>.IPermission<TInput>> permissionSelector, TInput input)
            where TPolicy : SecurityPolicy<T>
        {
            return WhenAll(a => a.Has(permissionSelector, input)).Assert();
        }

        public void ThrowOnDeny<TPolicy>(Func<TPolicy, SecurityPolicy<T>.IPermission> permissionSelector)
            where TPolicy : SecurityPolicy<T>
        {
            WhenAll(a => a.Has(permissionSelector)).ThowOnDeny();
        }

        public void ThrowOnDeny<TPolicy, TInput>(Func<TPolicy, SecurityPolicy<T>.IPermission<TInput>> permissionSelector, TInput input)
            where TPolicy : SecurityPolicy<T>
        {
            WhenAll(a => a.Has(permissionSelector, input)).ThowOnDeny();
        }

        public PolicyAssertion<T> WhenAll(Func<AssertionContainer<T>, AssertionContainer<T>> assertions)
        {
            return new PolicyAssertion<T>(this, userSecurityContext, calculationStrategy).AndAll(assertions);
        }

        public PolicyAssertion<T> WhenAny(Func<AssertionContainer<T>, AssertionContainer<T>> assertions)
        {
            return new PolicyAssertion<T>(this, userSecurityContext, calculationStrategy).AndAny(assertions);
        }
    }
}
