using System;

namespace FluentAuthorization
{
    public class PolicyAssertionException : Exception
    {
        public AssertionResult Result { get; set; }

        public PolicyAssertionException(AssertionResult assertionResult)
            :base("A security policy assertion denied access due to the following reasons: " + string.Join(",", assertionResult.Reasons))
        {
        }
    }
}
