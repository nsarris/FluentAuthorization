using System;

namespace FluentAuthorization
{

    public sealed class PolicyAssertion<TUserSecurityContext> : IAssertable
    {
        readonly UserSecuritySchema<TUserSecurityContext> policyProvider;
        readonly IPermissionCalculationStrategy calculationStrategy;
        readonly AssertionContainer<TUserSecurityContext> assertionContainer;
        readonly TUserSecurityContext userSecurityContext;
        internal PolicyAssertion(UserSecuritySchema<TUserSecurityContext> policyProvider, TUserSecurityContext userSecurityContext, IPermissionCalculationStrategy calculationStrategy)
        {
            this.policyProvider = policyProvider;
            this.calculationStrategy = calculationStrategy;
            this.userSecurityContext = userSecurityContext;

            assertionContainer = new AssertionContainer<TUserSecurityContext>(policyProvider, userSecurityContext, LogicalOperator.And, calculationStrategy);
        }

        public PolicyAssertion<TUserSecurityContext> AndAll(Func<AssertionContainer<TUserSecurityContext>, AssertionContainer<TUserSecurityContext>> assertions)
        {
            assertionContainer.Has(x =>
                new ResolvedPolicyAssertion(assertions(new AssertionContainer<TUserSecurityContext>(policyProvider, userSecurityContext, LogicalOperator.And, calculationStrategy)).Assert()));
            return this;
        }

        public PolicyAssertion<TUserSecurityContext> AndAny(Func<AssertionContainer<TUserSecurityContext>, AssertionContainer<TUserSecurityContext>> assertions)
        {
            assertionContainer.Has(x =>
                new ResolvedPolicyAssertion(assertions(new AssertionContainer<TUserSecurityContext>(policyProvider, userSecurityContext, LogicalOperator.Or, calculationStrategy)).Assert()));
            return this;
        }

        //public bool Assert()
        //{
        //    return Assert().Allow;
        //}

        public AssertionResult Assert()
        {
            return assertionContainer.Assert();
        }

        public void ThowOnDeny()
        {
            var assertionResult = assertionContainer.Assert();
            if (assertionResult.Deny)
                throw new PolicyAssertionException(assertionResult);
        }

        public PolicyAssertion<TUserSecurityContext> WithError(Func<string> errorBuilder)
        {
            assertionContainer.WithError(errorBuilder);
            return this;
        }
    }
}

