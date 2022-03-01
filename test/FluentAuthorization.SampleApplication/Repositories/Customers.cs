using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleApplication.Model
{
    static class Customers
    {
        static List<Customer> customers;
        static List<Product> products;
        static Customers()
        {
            products = new List<Product>()
            {
                new Product() { Id = 1, Name = "Savings account"},
                new Product() { Id = 2, Name = "Business account"},
                new Product() { Id = 3, Name = "Payroll account"},
                new Product() { Id = 4, Name = "Loan"}
            };

            customers = new List<Customer>
            {
                new Customer{
                    Id = 1,
                    Name = "Johnny Walker",
                    IsVip = true,
                    IsPersonnel = false,
                    Accounts = new List<Account>()
                    {
                        new Account() { Id = 11, Product = products[0], Balance = 1000 },
                        new Account() { Id = 12, Product = products[1], Balance = 2000 },
                    }

                },
                new Customer{
                    Id = 2,
                    Name = "Jim Beam",
                    IsVip = false,
                    IsPersonnel = true,
                    Accounts = new List<Account>()
                    {
                        new Account() { Id = 21, Product = products[2], Balance = 4000 },
                    }
                },
                new Customer{
                    Id = 3,
                    Name = "Jack Daniels",
                    IsVip = false,
                    IsPersonnel = false,
                    Accounts = new List<Account>()
                    {
                        new Account() { Id = 31, Product = products[0], Balance = 1000 },
                        new Account() { Id = 32, Product = products[1], Balance = 2000 },
                        new Account() { Id = 33, Product = products[3], Balance = 3000 },
                    }
                },

            };
        }
        public static List<Customer> Get()
        {
            return customers;
        }
    }
}
