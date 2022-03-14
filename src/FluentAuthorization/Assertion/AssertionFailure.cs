namespace FluentAuthorization
{
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

        public string User { get; }
        public string Permission { get; }
        public string Policy { get; }
        public string Message { get; }
        public string Reason { get; }

        public override string ToString()
        {
            return DefaultMessageBuilder.BuildFailureMessage(Message, Reason);
        }
    }
}
