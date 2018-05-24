using FluentAuthorization;
using System;

namespace Authorization
{
    public enum PolicyEnum
    {
        AccessData = 0,
        ValueProcessing = 1,
    }

    public class PolicyIdAttribute : Attribute
    {
        public PolicyEnum PolicyId { get; private set; }
        public PolicyIdAttribute(PolicyEnum id)
        {
            PolicyId = id;
        }
    }


    public abstract class MyPolicy : SecurityPolicy<MyUserSecurityContext>
    {
        public PolicyEnum Id { get; private set; }   //set from attribute on dictionary create
    }


    public abstract class MyPolicy<T> : SecurityPolicy<T, MyUserSecurityContext>
       where T : PolicyData
    {
        public MyPolicy(T data) : base(data)
        {
        }

        public PolicyEnum Id { get; private set; }   //set from attribute on dictionary create
    }
}
