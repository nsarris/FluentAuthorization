using FluentAuthorization;
using SampleApplication.Authorization.Policies;
using SampleApplication.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleApplication.Authorization.Repositories
{
    public class CustomerRepository
    {
        private readonly List<Customer> customers = Customers.Get();

        private readonly IPolicyContextProvider<Principal> policyContextProvider;

        public CustomerRepository(IPolicyContextProvider<Principal> policyContextProvider)
        {
            this.policyContextProvider = policyContextProvider;
        }

        private Task<IPolicyContext<CustomerPolicy>> GetSecurityContext()
            => policyContextProvider
                //.ForUser(user)
                .ForResource(EntityTypeResource.Customer)
                .ForPolicy<CustomerPolicy>()
                .BuildContextAsync();

        public async Task CreateAsync(Customer customer)
        {
            var policyContext = await GetSecurityContext();

            policyContext.ThrowOnDeny(x => x.Create);

            customers.Add(customer);
        }

        public async Task DeleteAsync(Customer customer)
        {
            var policyContext = await GetSecurityContext();

            policyContext.ThrowOnDeny(x => x.Delete);

            customers.RemoveAll(x => x.Id == customer.Id);
        }

        public async Task UpdateAsync(Customer customer)
        {
            var policyContext = await GetSecurityContext();

            policyContext.ThrowOnDeny(x => x.Update);

            customers.RemoveAll(x => x.Id == customer.Id);

            customers.Add(customer);
        }

        public async Task<Customer> GetByIdAsync(int id)
        {
            var policyContext = await GetSecurityContext();

            policyContext.ThrowOnDeny(x => x.View);

            var customer = customers.FirstOrDefault(x => x.Id == id);

            policyContext.ThrowOnDeny(x => x.ViewCustomer, customer);

            if (!policyContext.Assert(x => x.ViewRealName, customer))
                customer.Name = "Name obfuscated";

            return customer;
        }

        public async Task<List<Customer>> GetAsync()
        {
            var policyContext = await GetSecurityContext();

            return customers
                .Where(customer => policyContext.Assert(x => x.ViewCustomer, customer))
                .Select(customer =>
                {
                    if (!policyContext.Assert(x => x.ViewRealName, customer))
                        customer.Name = "Name obfuscated";
                    return customer;
                })
                .ToList();
        }
    }
}
