using System;

namespace FluentAuthorization
{
    /// <summary>
    /// An exception thrown when a policy permission assertion fails.
    /// </summary>
    public class PolicyAssertionException : Exception
    {
        /// <summary>
        /// The aggegrated failed result.
        /// </summary>
        public AssertionResult Result { get; set; }

        public PolicyAssertionException(AssertionResult assertionResult)
            :base("Security policy assertion failure: " + string.Join(Environment.NewLine, assertionResult.Failures))
        {
        }
    }
}
