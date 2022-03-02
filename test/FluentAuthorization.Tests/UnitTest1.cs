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

            var c = await customerRepo.GetByIdAsync(1);

            Assert.True(c.Id == 1);

            await Assert.ThrowsAsync<PolicyAssertionException>(() => customerRepo.GetByIdAsync(2));
            await Assert.ThrowsAsync<PolicyAssertionException>(() => customerRepo.GetByIdAsync(3));
            

            await Assert.ThrowsAsync<PolicyAssertionException>(() => customerRepo.UpdateAsync(c));
        }
    }
}