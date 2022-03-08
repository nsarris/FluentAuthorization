using SampleApplication.Authorization;
using SampleApplication.Authorization.Policies;
using System;
using Xunit;

namespace FluentAuthorization.Tests
{
    public class TestFixture
    {
        public Principal User { get; }
        public IUserContextProvider<Principal> UserContextProvider { get; }
        public IPolicyDataProvider<Principal> DataProvider { get; }
        
        public IPolicyContextProvider PolicyContextProvider { get; }

        public TestFixture()
        {
            User = new Principal("user1", new[] { "g1", "g2" }, new RolesEnum[] { RolesEnum.Cashier });
            //PolicyProvider = new TestPolicyProvider();
            DataProvider = new TestDataProvider();
            UserContextProvider = new TestUserContextProvider(User);

            PolicyContextProvider = new PolicyContextProvider(
                UserContextProvider, 
                DataProvider);
        }
    }


    [CollectionDefinition(nameof(TestFixture))]
    public class TestCollection : ICollectionFixture<TestFixture>
    {

    }
}