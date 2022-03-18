namespace FluentAuthorization
{
    /// <summary>
    /// Encapsulates a set of strings that describe an assertion failure.
    /// </summary>
    public class AssertionFailure
    {
        public AssertionFailure(string user, string permissionName, string policyName, string message, string reason)
        {
            User = user;
            Permission = permissionName;
            Policy = policyName;
            Message = message;
            Reason = reason;
        }

        /// <summary>
        /// A string representation of the user related to the assertion.
        /// </summary>
        public string User { get; }

        /// <summary>
        /// A string representation of the permission asserted.
        /// </summary>
        public string Permission { get; }

        /// <summary>
        /// A string representation of the policy that contains the permission asserted.
        /// </summary>
        public string Policy { get; }

        /// <summary>
        /// A message related with the assertion failure.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// An optional description of the reason of the assertion failure.
        /// </summary>
        public string Reason { get; }

        public override string ToString()
        {
            return DefaultMessageBuilder.BuildFailureMessage(Message, Reason);
        }
    }
}
