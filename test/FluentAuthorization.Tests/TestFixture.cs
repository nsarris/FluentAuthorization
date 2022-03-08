using SampleApplication.Authorization;
using SampleApplication.Authorization.Policies;
using System;
using Xunit;

namespace FluentAuthorization.Tests
{
    public class TestFixture
    {
        public MyUser User { get; }
        public IUserContextProvider<MyUser> UserContextProvider { get; }
        public IPolicyDataProvider<MyUser> DataProvider { get; }
        
        public IPolicyContextProvider PolicyContextProvider { get; }

        public TestFixture()
        {
            User = new MyUser("user1", new[] { "g1", "g2" }, new RolesEnum[] { RolesEnum.Cashier });
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