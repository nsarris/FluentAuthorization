using SampleApplication.Authorization;
using SampleApplication.Authorization.Repositories;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using SampleApplication.Authorization.Policies;

namespace FluentAuthorization.Tests
{
    [Collection(nameof(DependencyInjectionTestFixture))]
    public class UnitTest2 : IClassFixture<DependencyInjectionTestFixture>
    {
        private readonly DependencyInjectionTestFixture fixture;

        public UnitTest2(DependencyInjectionTestFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public async Task Test1()
        {
            var serviceProvider = fixture.BuildServiceProvider(services =>
            {
                services.AddTransient<CustomerRepository>();
            });

            var customerRepo = serviceProvider.GetRequiredService<CustomerRepository>();

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