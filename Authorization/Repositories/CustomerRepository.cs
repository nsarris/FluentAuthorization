using Authorization.Model;
using Authorization.Policies;
using FluentAuthorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authorization.Repositories
{
    public class CustomerRepository
    {
        private List<Customer> customers = Customers.Get();

        private readonly UserSecuritySchema<MyUserSecurityContext> userSecuritySchema;

        public CustomerRepository(UserSecuritySchema<MyUserSecurityContext> userSecuritySchema)
        {
            this.userSecuritySchema = userSecuritySchema;
        }

        public void Create(Customer customer)
        {
            userSecuritySchema.Throw<CustomerPolicy>(x => x.Create);

            customers.Add(customer);
        }

        public void Delete(Customer customer)
        {
            userSecuritySchema.Throw<CustomerPolicy>(x => x.Delete);

            customers.RemoveAll(x => x.Id == customer.Id);
        }

        public void Update(Customer customer)
        {
            userSecuritySchema.Throw<CustomerPolicy>(x => x.Update);

            customers.RemoveAll(x => x.Id == customer.Id);

            customers.Add(customer);
        }

        private Customer ApplySecurityFilter(Customer customer)
        {
            if (userSecuritySchema.Assert((CustomerPolicy x) => x.ViewRealName, customer).Deny)
                customer.Name = "Name obfuscated";

            return customer;
        }

        public Customer GetById(int id)
        {
            var customer = customers.Where(x => x.Id == id).FirstOrDefault();

            userSecuritySchema.Throw((CustomerPolicy x) => x.ViewCustomer, customer);

            ApplySecurityFilter(customer);

            return customer;
        }

        public List<Customer> Get()
        {
            userSecuritySchema.WhenAll(a => a.Has<CustomerPolicy>(x => x.View)).Throw();

            return customers
                .Where(customer =>
                        userSecuritySchema
                        .Assert((CustomerPolicy x) => x.ViewCustomer, customer).Allow)
                .Select(ApplySecurityFilter)
                .ToList();
        }
    }
}
