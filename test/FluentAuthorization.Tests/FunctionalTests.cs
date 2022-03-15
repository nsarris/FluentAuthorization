using SampleApplication.Authorization;
using SampleApplication.Authorization.Repositories;
using Xunit;

namespace FluentAuthorization.Tests
{

    [Collection(nameof(TestFixture))]
    public class FunctionalTests : FunctionalTestsBase, IClassFixture<TestFixture>
    {
        private readonly TestFixture fixture;

        public FunctionalTests(TestFixture fixture)
        {
            this.fixture = fixture;
        }

        public override CustomerRepository GetCustomerRepository()
            => new CustomerRepository(fixture.PolicyContextProvider);

        public override IPolicyContextProvider GetPolicyContextProvider()
            => fixture.PolicyContextProvider;
    }
}