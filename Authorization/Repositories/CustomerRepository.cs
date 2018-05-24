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
        private List<Customer> customers = new List<Customer>();
        private readonly UserSecuritySchema<MyUserSecurityContext> userSecuritySchema;

        public CustomerRepository(UserSecuritySchema<MyUserSecurityContext> userSecuritySchema)
        {
            this.userSecuritySchema = userSecuritySchema;
        }

        public void Create(Customer customer)
        {
            userSecuritySchema.WhenAll(a => a.Has<CustomerPolicy>(x => x.Create)).Throw();

            customers.Add(customer);
        }

        public void Delete(Customer customer)
        {
            userSecuritySchema.WhenAll(a => a.Has<CustomerPolicy>(x => x.Delete)).Throw();

            customers.RemoveAll(x => x.Id == customer.Id);
        }

        public void Update(Customer customer)
        {
            userSecuritySchema.WhenAll(a => a.Has<CustomerPolicy>(x => x.Update)).Throw();

            customers.RemoveAll(x => x.Id == customer.Id);

            customers.Add(customer);
        }

        private Customer ApplySecurityFilter(Customer customer)
        {
            if (userSecuritySchema.WhenAll(a => a.Has((CustomerPolicy x) => x.ViewRealName, customer)).Assert().Deny)
                customer.Name = "Name obfuscated";

            return customer;
        }

        public Customer GetById(int id)
        {
            var customer = customers.Where(x => x.Id == id).FirstOrDefault();

            userSecuritySchema.WhenAll(a => a.Has((CustomerPolicy x) => x.ViewCustomer, customer)).Throw();

            ApplySecurityFilter(customer);

            return customer;
        }

        public List<Customer> Get()
        {
            userSecuritySchema.WhenAll(a => a.Has<CustomerPolicy>(x => x.View)).Throw();

            return customers
                .Where(customer =>
                        userSecuritySchema
                        .WhenAll(a => a.Has((CustomerPolicy x) => x.ViewCustomer, customer))
                        .Assert().Allow)
                .Select(ApplySecurityFilter)
                .ToList();
        }
    }
}
