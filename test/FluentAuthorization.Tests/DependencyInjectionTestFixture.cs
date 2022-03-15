using Microsoft.Extensions.DependencyInjection;
using SampleApplication.Authorization;
using FluentAuthorization.DependencyInjection;
using Xunit;
using System;

namespace FluentAuthorization.Tests
{
    public class DependencyInjectionTestFixture
    {
        public Principal User { get; }
        
        public IServiceProvider DefaultServiceProvider { get; }
        
        public DependencyInjectionTestFixture()
        {
            User = new Principal("user1", "John Doe", new Roles[] { Roles.Cashier });

            var services = new ServiceCollection();

            BuildServices(services);

            DefaultServiceProvider = BuildServiceProvider(_ => { });
        }

        public IServiceProvider BuildServiceProvider(Action<IServiceCollection> configure)
        {
            var services = new ServiceCollection();
            BuildServices(services);
            configure?.Invoke(services);
            return services.BuildServiceProvider();
        }

        private void BuildServices(IServiceCollection services)
        {
            services.AddFluentAuthorization<Principal>(c => c
                   .AddUserContextProvider(sp => new TestUserContextProvider(User), ServiceLifetime.Singleton)
                   .AddDataProvider<TestDataProvider>(ServiceLifetime.Transient)

                   .AddCustomPolicyContextProvider<IPolicyContextProvider, PolicyContextProvider>() //optional
               );
        }
    }

    [CollectionDefinition(nameof(DependencyInjectionTestFixture))]
    public class DependencyInjectionTestCollection : ICollectionFixture<DependencyInjectionTestFixture>
    {

    }
}