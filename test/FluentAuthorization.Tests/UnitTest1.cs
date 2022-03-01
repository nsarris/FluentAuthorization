using SampleApplication.Authorization;
using SampleApplication.Authorization.Repositories;
using System.Threading.Tasks;
using Xunit;

namespace FluentAuthorization.Tests
{
    [Collection(nameof(TestFixture))]
    public class UnitTest1 : IClassFixture<TestFixture>
    {
        private readonly TestFixture fixture;

        public UnitTest1(TestFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public async Task Test1()
        {
            var customerRepo = new CustomerRepository(fixture.PolicyContextProvider);

            var customers = await customerRepo.GetAsync();
            Assert.True(customers.Count == 1);

            await Assert.ThrowsAsync<PolicyAssertionException>(() => customerRepo.GetByIdAsync(1));
            await Assert.ThrowsAsync<PolicyAssertionException>(() => customerRepo.GetByIdAsync(3));
            var c2 = await customerRepo.GetByIdAsync(2);

            Assert.True(c2.Id == 2);

            await Assert.ThrowsAsync<PolicyAssertionException>(() => customerRepo.UpdateAsync(c2));
        }
    }
}