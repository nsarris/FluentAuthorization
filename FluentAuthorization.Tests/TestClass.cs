using Authorization;
using Authorization.Repositories;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentAuthorization.Tests
{
    [TestFixture]
    public class TestClass
    {
        [Test]
        public void TestMethod()
        {
            var securityContext = UserContexts.User1;
            var customerRepo = new CustomerRepository(securityContext);

            var customers = customerRepo.Get();
            Assert.True(customers.Count == 1);

            Assert.Throws<PolicyAssertionException>(() => customerRepo.GetById(1));
            Assert.Throws<PolicyAssertionException>(() => customerRepo.GetById(3));
            var c2 = customerRepo.GetById(2);

            Assert.True(c2.Id == 2);

            Assert.Throws<PolicyAssertionException>(() => customerRepo.Update(c2));
        }
    }
}
