using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentAuthorization
{
    public partial class UserSecuritySchema<TUserSecurityContext>
    {
        readonly TUserSecurityContext userSecurityContext;
        readonly List<SecurityPolicy<TUserSecurityContext>> policies = new List<SecurityPolicy<TUserSecurityContext>>();
        readonly IPolicyRepository<TUserSecurityContext> policyRepository;
        readonly IPermissionCalculationStrategy calculationStrategy;
        
        public UserSecuritySchema(
            IPolicyRepository<TUserSecurityContext> policyRepository, 
            TUserSecurityContext userSecurityContext)
        {
            this.policyRepository = policyRepository;
            this.userSecurityContext = userSecurityContext;
            calculationStrategy = new PermissionCalculationMergePermissionStrategy();
        }

        public UserSecuritySchema(
            IPolicyRepository<TUserSecurityContext> policyRepository,
            TUserSecurityContext securityContext,
            IPermissionCalculationStrategy calculationStrategy)
            :this(policyRepository, securityContext)
        {
            this.calculationStrategy = calculationStrategy;
        }


        public IEnumerable<TPolicy> GetPolicies<TPolicy>() 
            where TPolicy : SecurityPolicy<TUserSecurityContext>
        {
            return policyRepository.GetByUserSecurityContext(userSecurityContext).OfType<TPolicy>();
        }


        public AssertionResult Assert<TPolicy>(Func<TPolicy, SecurityPolicy<TUserSecurityContext>.IPermission> permissionSelector)
            where TPolicy : SecurityPolicy<TUserSecurityContext>
        {
            return WhenAll(a => a.Has(permissionSelector)).Assert();
        }

        public AssertionResult Assert<TPolicy, TInput>(Func<TPolicy, SecurityPolicy<TUserSecurityContext>.IPermission<TInput>> permissionSelector, TInput input)
            where TPolicy : SecurityPolicy<TUserSecurityContext>
        {
            return WhenAll(a => a.Has(permissionSelector, input)).Assert();
        }

        public void Throw<TPolicy>(Func<TPolicy, SecurityPolicy<TUserSecurityContext>.IPermission> permissionSelector)
            where TPolicy : SecurityPolicy<TUserSecurityContext>
        {
            WhenAll(a => a.Has(permissionSelector)).Throw();
        }

        public void Throw<TPolicy, TInput>(Func<TPolicy, SecurityPolicy<TUserSecurityContext>.IPermission<TInput>> permissionSelector, TInput input)
            where TPolicy : SecurityPolicy<TUserSecurityContext>
        {
            WhenAll(a => a.Has(permissionSelector, input)).Throw();
        }



        public PolicyAssertion<TUserSecurityContext> WhenAll(Func<AssertionContainer<TUserSecurityContext>, AssertionContainer<TUserSecurityContext>> assertions)
        {
            return new PolicyAssertion<TUserSecurityContext>(this, userSecurityContext, calculationStrategy).AndAll(assertions);
        }

        public PolicyAssertion<TUserSecurityContext> WhenAny(Func<AssertionContainer<TUserSecurityContext>, AssertionContainer<TUserSecurityContext>> assertions)
        {
            return new PolicyAssertion<TUserSecurityContext>(this, userSecurityContext, calculationStrategy).AndAny(assertions);
        }
    }
}
