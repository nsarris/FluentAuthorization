using System;

namespace FluentAuthorization
{
    public class PermissionDenialMessageAttribute : Attribute
    {
        public string Message { get; }
        public PermissionDenialMessageAttribute(string message)
        {
            Message = message;
        }
    }
}
