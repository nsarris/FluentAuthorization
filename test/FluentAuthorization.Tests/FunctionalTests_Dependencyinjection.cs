using SampleApplication.Authorization;
using SampleApplication.Authorization.Repositories;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using SampleApplication.Authorization.Policies;
using System;

namespace FluentAuthorization.Tests
{
    [Collection(nameof(DependencyInjectionTestFixture))]
    public class FunctionalTests_DependencyInjection : FunctionalTestsBase, IClassFixture<DependencyInjectionTestFixture>
    {
        private readonly DependencyInjectionTestFixture fixture;
        private readonly IServiceProvider serviceProvider;

        public FunctionalTests_DependencyInjection(DependencyInjectionTestFixture fixture)
        {
            this.fixture = fixture;
            serviceProvider = fixture.BuildServiceProvider(services =>
            {
                services.AddTransient<CustomerRepository>();
            });
        }

        public override CustomerRepository GetCustomerRepository()
            => serviceProvider.GetRequiredService<CustomerRepository>();


        public override IPolicyContextProvider GetPolicyContextProvider()
             => serviceProvider.GetRequiredService<IPolicyContextProvider>();
    }
}