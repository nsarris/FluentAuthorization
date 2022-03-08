using SampleApplication.Authorization;
using SampleApplication.Authorization.Policies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FluentAuthorization.Tests
{
    class TestDataProvider : IPolicyDataProvider<MyUserSecurityContext>
    {
        public Task<IEnumerable<TData>> GetDataAsync<TPolicy, TResource, TData>(MyUserSecurityContext user, TPolicy policy, TResource resource)
            where TPolicy : IPolicy<MyUserSecurityContext, TResource, TData>
        {
            return Task.FromResult(policy switch
            {
                CustomerPolicy customerPolicy when resource is EntityTypeResource entityType =>
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
                    }.Cast<TData>(),
                CustomerAccountPolicy accountPolicy when resource is EntityTypeResource entityType =>
                    new[]
                    {
                            new CustomerAccountPolicy.CustomerAccountPolicyData(true,true, true, true, 0)
                    }.Cast<TData>(),
                _ => throw new InvalidOperationException("Policy not supported")
            });
        }
    }
}