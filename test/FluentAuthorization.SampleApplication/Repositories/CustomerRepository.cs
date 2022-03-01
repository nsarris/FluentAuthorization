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
        private List<Customer> customers = Customers.Get();

        private readonly IPolicyContextProvider<MyUserSecurityContext> policyContextProvider;

        public CustomerRepository(IPolicyContextProvider<MyUserSecurityContext> policyContextProvider)
        {
            this.policyContextProvider = policyContextProvider;
        }

        private Task<IPolicyContext<CustomerPolicy>> GetSecurityContext()
            => policyContextProvider
                .ForUser(null)
                .ForResource(EntityTypeResource.Customer)
                .ForPolicy<CustomerPolicy>()
                .BuildContextAsync();

        public async Task CreateAsync(Customer customer)
        {
            //policyContextProvider.ThrowOnDeny<CustomerPolicy>(x => x.Create);

            var policyContext = await GetSecurityContext();

            if (!policyContext.Assert(x => x.Create))
                throw new InvalidOperationException();

            customers.Add(customer);
        }

        public async Task DeleteAsync(Customer customer)
        {
            //policyContextProvider.ThrowOnDeny<CustomerPolicy>(x => x.Delete);

            var policyContext = await GetSecurityContext();

            if (!policyContext.Assert(x => x.Delete))
                throw new InvalidOperationException();

            customers.RemoveAll(x => x.Id == customer.Id);
        }

        public async Task UpdateAsync(Customer customer)
        {
            //policyContextProvider.ThrowOnDeny<CustomerPolicy>(x => x.Update);

            var policyContext = await GetSecurityContext();

            if (!policyContext.Assert(x => x.Update))
                throw new InvalidOperationException();

            customers.RemoveAll(x => x.Id == customer.Id);

            customers.Add(customer);
        }

        //private Customer ApplySecurityFilter(Customer customer)
        //{
        //    var policyContext = await policyContextProvider
        //        .ForResource(EntityTypeResource.Customer)
        //        .ForPolicy<CustomerPolicy>()
        //        .BuildContextAsync();


        //    if (
        //        //.Assert((CustomerPolicy x) => x.ViewRealName, customer).Deny)
        //        customer.Name = "Name obfuscated";

        //    return customer;
        //}

        public async Task<Customer> GetByIdAsync(int id)
        {
            var policyContext = await GetSecurityContext();

            if (!policyContext.Assert(x => x.View))
                throw new InvalidOperationException();

            var customer = customers.Where(x => x.Id == id).FirstOrDefault();

            if (!policyContext.Assert(x => x.ViewCustomer, customer))
                throw new InvalidOperationException();

            //ApplySecurityFilter(customer);

            if (!policyContext.Assert(x => x.ViewRealName, customer))
                customer.Name = "Name obfuscated";

            return customer;
        }

        public async Task<List<Customer>> GetAsync()
        {
            //policyContextProvider.WhenAll(a => a.Has<CustomerPolicy>(x => x.View)).ThowOnDeny();

            //return customers
            //    .Where(customer =>
            //            policyContextProvider
            //            .Assert((CustomerPolicy x) => x.ViewCustomer, customer).Allow)
            //    .Select(ApplySecurityFilter)
            //    .ToList();

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
