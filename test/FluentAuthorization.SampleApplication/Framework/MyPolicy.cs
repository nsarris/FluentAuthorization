using FluentAuthorization;
using System;

namespace SampleApplication.Authorization
{
    public abstract class BasePolicy<TResource, TData> : Policy<MyUser, TResource, TData>
    {
    
    }

    public interface IPolicyContextProvider : IPolicyContextProvider<MyUser> { }

    public class PolicyContextProvider : PolicyContextProvider<MyUser>, IPolicyContextProvider
    {
        public PolicyContextProvider(
            IUserContextProvider<MyUser> userContextProvider, 
            IPolicyDataProvider<MyUser> dataProvider) 
            : base(userContextProvider, dataProvider)
        {
        }
    }
}
