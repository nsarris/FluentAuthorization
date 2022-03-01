using System;

namespace FluentAuthorization
{
    public class PermissionNameAttribute : Attribute
    {
        public string Name { get; }
        public PermissionNameAttribute(string name)
        {
            Name = name;
        }
    }
}
