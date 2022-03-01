using SampleApplication.Authorization;
using SampleApplication.Authorization.Policies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace FluentAuthorization.Tests
{
    public class TestFixture
    {
        public MyUserSecurityContext User { get; }
        public IUserContextProvider<MyUserSecurityContext> UserContextProvider { get; }
        public IPolicyDataProvider<MyUserSecurityContext> DataProvider { get; }
        public IPolicyProvider PolicyProvider { get; }

        public IPolicyContextProvider PolicyContextProvider { get; }

        public TestFixture()
        {
            User = new MyUserSecurityContext("user1", new[] { "g1", "g2" }, new RolesEnum[] { RolesEnum.Cashier });
            PolicyProvider = new TestPolicyProvider();
            DataProvider = new TestDataProvider();
            UserContextProvider = new TestUserContextProvider(User);

            PolicyContextProvider = new PolicyContextProvider(UserContextProvider, PolicyProvider, DataProvider, null);
        }

        class TestUserContextProvider : IUserContextProvider<MyUserSecurityContext>
        {
            private readonly MyUserSecurityContext user;

            public TestUserContextProvider(MyUserSecurityContext user)
            {
                this.user = user;
            }

            public Task<MyUserSecurityContext> GetAsync()
            {
                return Task.FromResult(user);
            }
        }

        class TestPolicyProvider : IPolicyProvider
        {
            public T Get<T>() where T : class, IPolicy
            {
                if (typeof(T) == typeof(CustomerPolicy))
                    return (new CustomerPolicy() as T)!;

                throw new InvalidOperationException("Policy not supported");
            }
        }

        class TestDataProvider : IPolicyDataProvider<MyUserSecurityContext>
        {
            public Task<IEnumerable<TData>> GetDataAsync<TPolicy, TResource, TData>(MyUserSecurityContext user, TPolicy policy, TResource resource) where TPolicy : IPolicy<TResource, TData>
            {
                if (policy is CustomerPolicy customerPolicy)
                {
                    return Task.FromResult(
                        new[] {
                            new CustomerPolicy.CustomerPolicyData(
                            create: false,
                            delete: false,
                            view: true,
                            update: false,
                            viewPersonnel: false,
                            viewVip: true,
                            viewBalanceLimit: 5000,
                            viewRealNames: false)
                     }
                    .Cast<TData>());
                }

                throw new InvalidOperationException("Policy not supported");
            }
        }
    }


    [CollectionDefinition(nameof(TestFixture))]
    public class TestCollection : ICollectionFixture<TestFixture>
    {

    }
}