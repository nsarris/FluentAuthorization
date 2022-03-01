namespace FluentAuthorization
{
    internal class ResolvedPolicyAssertion : IAssertable
    {
        AssertionResult result;
        public ResolvedPolicyAssertion(AssertionResult result)
        {
            this.result = result;
        }

        public AssertionResult Assert()
        {
            return result;
        }
    }
}
