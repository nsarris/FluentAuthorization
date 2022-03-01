using System;

namespace FluentAuthorization
{
    public class PolicyNameAttribute : Attribute
    {
        public string Name { get; }
        public PolicyNameAttribute(string name)
        {
            Name = name;
        }
    }

}
