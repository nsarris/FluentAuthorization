using System;
using System.Runtime.Serialization;

namespace FluentAuthorization
{
    /// <summary>
    /// An exception thrown when a policy permission assertion fails.
    /// </summary>
    [Serializable]
    public sealed class PolicyAssertionException : Exception
    {
        /// <summary>
        /// The aggegrated failed result.
        /// </summary>
        public AssertionResult Result { get; set; }

        public PolicyAssertionException(AssertionResult result)
            : base("Security policy assertion failure: " + string.Join(Environment.NewLine, result.Failures))
        {
            Result = result;
        }

        private PolicyAssertionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
