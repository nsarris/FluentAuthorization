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
