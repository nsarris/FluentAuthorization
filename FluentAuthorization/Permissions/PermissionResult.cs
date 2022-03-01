namespace FluentAuthorization
{
    public class PermissionResult
    {
        public bool Allow { get; set; }
        public string DenialMessage { get; set; }
        public object Metadata { get; set; }
        public object Input { get; set; }
    }
}
