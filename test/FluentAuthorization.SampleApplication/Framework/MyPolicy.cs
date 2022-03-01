using FluentAuthorization;
using System;

namespace SampleApplication.Authorization
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


    public abstract class BasePolicy<TResource, TData> : Policy<MyUserSecurityContext, TResource, TData>
    {
        public PolicyEnum Id { get; private set; }   //set from attribute on dictionary create
    }


    public interface IPolicyContextProvider : IPolicyContextProvider<MyUserSecurityContext> { }

    public class PolicyContextProvider : PolicyContextProvider<MyUserSecurityContext>, IPolicyContextProvider
    {
        public PolicyContextProvider(
            IUserContextProvider<MyUserSecurityContext> userContextProvider, 
            IPolicyProvider policyProvider, 
            IPolicyDataProvider<MyUserSecurityContext> dataProvider, 
            IServiceProvider serviceProvider = null) 
            : base(userContextProvider, policyProvider, dataProvider, serviceProvider)
        {
        }
    }
}
