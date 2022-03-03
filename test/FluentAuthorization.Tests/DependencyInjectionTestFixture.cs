using Microsoft.Extensions.DependencyInjection;
using SampleApplication.Authorization;
using FluentAuthorization.DependencyInjection;
using Xunit;
using System;

namespace FluentAuthorization.Tests
{
    public class DependencyInjectionTestFixture
    {
        public MyUserSecurityContext User { get; }
        
        public IServiceProvider DefaultServiceProvider { get; }
        
        public DependencyInjectionTestFixture()
        {
            User = new MyUserSecurityContext("user1", new[] { "g1", "g2" }, new RolesEnum[] { RolesEnum.Cashier });

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
            services.AddFluentAuthorization<MyUserSecurityContext>(c => c
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