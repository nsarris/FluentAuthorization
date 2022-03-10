using System;

namespace FluentAuthorization
{
    public class PolicyAssertionException : Exception
    {
        public AssertionResult Result { get; set; }

        public PolicyAssertionException(AssertionResult assertionResult)
            :base("Security policy assertion failure: " + string.Join(Environment.NewLine, assertionResult.Failures))
        {
        }
    }
}
