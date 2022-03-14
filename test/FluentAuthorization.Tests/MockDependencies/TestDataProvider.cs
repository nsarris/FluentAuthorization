using SampleApplication.Authorization;
using SampleApplication.Authorization.Policies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FluentAuthorization.Tests
{
    class TestDataProvider : IPolicyDataProvider<Principal>
    {
        public Task<IEnumerable<TData>> GetDataAsync<TPolicy, TResource, TData>(Principal user, TPolicy policy, TResource resource)
            where TPolicy : IPolicy<Principal, TResource, TData>
        {
            return Task.FromResult(GetMockData<TPolicy, TResource, TData>(user, policy, resource).Cast<TData>());
        }

        private static IEnumerable<object> GetMockData<TPolicy, TResource, TData>(Principal user, TPolicy policy, TResource resource)
            where TPolicy : IPolicy<Principal, TResource, TData>
        {
            return policy switch
            {
                CustomerEntityPolicy customerEntityPolicy when resource is EntityTypeResource entityType => GetCustomerEntityPolicyData(),
                CustomerRecordPolicy customerRecordPolicy when resource is CustomerRecordResource recordResource => GetCustomerRecordPolicyData(recordResource),
                _ => throw new InvalidOperationException("Policy not supported")
            };
        }

        private static IEnumerable<CustomerEntityPolicy.Data> GetCustomerEntityPolicyData()
        {
            return new[] {
                new CustomerEntityPolicy.Data(
                create: false,
                delete: false,
                view: true,
                update: false,
                viewPersonnel: false,
                viewVip: true,
                viewBalanceLimit: 5000,
                viewRealNames: false)
            };
        }

        private static IEnumerable<RecordPolicyData> GetCustomerRecordPolicyData(CustomerRecordResource resource)
        {
            return resource.Id switch
            {
                1 => new RecordPolicyData[]
                {
                    new(create: false,
                        delete: false,
                        view: true,
                        update: false),
                },
                2 => new RecordPolicyData[]
                {
                    new(create: false,
                        delete: false,
                        view: false,
                        update: false),
                },
                3 => new RecordPolicyData[]
                {
                    new(create: false,
                        delete: false,
                        view: false,
                        update: false),
                },
                _ => throw new InvalidOperationException("Resource not found.")
            };
        }
    }
}