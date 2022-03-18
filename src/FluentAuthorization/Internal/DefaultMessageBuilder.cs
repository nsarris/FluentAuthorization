namespace FluentAuthorization
{
    internal static class DefaultMessageBuilder
    {
        public static string BuildMessage(string user, string policy, string permission)
        {
            return $"User {user} was denied permission {permission} of policy {policy}.";
        }

        public static string BuildFailureMessage(string message, string reason)
        {
            return string.IsNullOrEmpty(reason) ? message : $"{message} - Reason: {reason}";
        }
    }
}
