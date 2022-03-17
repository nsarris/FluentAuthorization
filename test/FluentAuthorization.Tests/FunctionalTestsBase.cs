using Moq;
using SampleApplication.Authorization;
using SampleApplication.Authorization.Policies;
using SampleApplication.Authorization.Repositories;
using SampleApplication.Model;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace FluentAuthorization.Tests
{
    public abstract class FunctionalTestsBase
    {
        public abstract IPolicyContextProvider GetPolicyContextProvider();
        public abstract CustomerRepository GetCustomerRepository();

        public Task<IPolicyContext<CustomerEntityPolicy>> GetCustomerPolicyContextAsync()
            => GetPolicyContextProvider()
                .ForResource(CustomerResource.Instance)
                .ForPolicy<CustomerEntityPolicy>()
                .BuildContextAsync();

        public Task<IPolicyContext<CustomerRecordPolicy>> GetCustomerRecordPolicyContextAsync(int id)
            => GetPolicyContextProvider()
                .ForResource(new CustomerRecordResource(id))
                .ForPolicy<CustomerRecordPolicy>()
                .BuildContextAsync();

        [Fact]
        public async Task Should_Filter_List()
        {
            var customerRepo = GetCustomerRepository();

            var customers = await customerRepo.GetAsync();
            Assert.True(customers.Count == 1);
        }

        [Fact]
        public async Task Should_Allow_To_Read()
        {
            var customerRepo = GetCustomerRepository();

            var c = await customerRepo.GetByIdAsync(1);
            Assert.True(c.Id == 1);
        }

        [Fact]
        public async Task Should_Fail_To_Read()
        {
            var customerRepo = GetCustomerRepository();

            await Assert.ThrowsAsync<PolicyAssertionException>(() => customerRepo.GetByIdAsync(2));
            await Assert.ThrowsAsync<PolicyAssertionException>(() => customerRepo.GetByIdAsync(3));
        }

        [Fact]
        public async Task Should_Fail_To_Write()
        {
            var customerRepo = GetCustomerRepository();
            var c = await customerRepo.GetByIdAsync(1);

            await Assert.ThrowsAsync<PolicyAssertionException>(() => customerRepo.UpdateAsync(c));
        }

        [Fact]
        public async Task Should_Fail_To_Read_With_Result()
        {
            var customers = Customers.Get();
            var policyContext = await GetCustomerPolicyContextAsync();

            var result = policyContext.Assert(x => x.ViewCustomer, customers[1]);

            Assert.False(result);
            Assert.True(result.Failures.Count() == 1);
            Assert.True(result.Failures.First().Permission == nameof(CustomerEntityPolicy.ViewCustomer));
            Assert.True(result.Failures.First().Policy == nameof(CustomerEntityPolicy));
            Assert.True(result.Failures.First().Reason is not null);
        }

        [Fact]
        public async Task Should_Fail_Read_Record_Resource()
        {
            var policyContext = await GetCustomerRecordPolicyContextAsync(1);

            var result = policyContext.Assert(x => x.View);

            Assert.True(result);
        }

        [Fact]
        public async Task Should_Fail_To_Read_Record_Resource()
        {
            var policyContext = await GetCustomerRecordPolicyContextAsync(2);

            var result = policyContext.Assert(x => x.View);

            Assert.False(result);
            Assert.True(result.Failures.Count() == 1);
            Assert.True(result.Failures.First().Permission == nameof(CustomerRecordPolicy.View));
            Assert.True(result.Failures.First().Policy == nameof(CustomerRecordPolicy));
            Assert.True(result.Failures.First().Reason == null);
        }
    }
}