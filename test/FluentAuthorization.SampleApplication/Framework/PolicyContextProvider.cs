using FluentAuthorization;

namespace SampleApplication.Authorization
{
    public class PolicyContextProvider : PolicyContextProvider<Principal>, IPolicyContextProvider
    {
        public PolicyContextProvider(
            IUserContextProvider<Principal> userContextProvider, 
            IPolicyDataProvider<Principal> dataProvider) 
            : base(userContextProvider, dataProvider)
        {
        }
    }
}
