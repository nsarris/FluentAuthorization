using Authorization;
using Authorization.Policies;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentAuthorization.Tests
{
    [SetUpFixture]
    public class TestSetup
    {
        [OneTimeSetUp]
        public void Setup()
        {
            var userPolicies = new PolicyLookup<string, MyUserSecurityContext>
            {
                {
                    "user1",
                    new CustomerPolicy(
                    new CustomerPolicy.CustomerPolicyData(
                        create: false,
                        delete: false,
                        view: true,
                        update: false,
                        viewPersonnel: false,
                        viewVip: true,
                        viewBalanceLimit: 5000,
                        viewRealNames: false
                        ))
                }
            };

            var groupPolicies = new PolicyLookup<string, MyUserSecurityContext>();
            var rolePolicies = new PolicyLookup<RolesEnum, MyUserSecurityContext>();

            var userPolicyRepo = new SecurityObjectPolicyRepository<string, MyUserSecurityContext>(userPolicies);
            var groupPolicyRepo = new SecurityObjectPolicyRepository<string, MyUserSecurityContext>(groupPolicies);
            var rolePolicyRepo = new SecurityObjectPolicyRepository<RolesEnum, MyUserSecurityContext>(rolePolicies);

            var policyRepo = new PolicyRepository(userPolicyRepo, groupPolicyRepo, rolePolicyRepo);

            var userSecurityContext1 = new MyUserSecurityContext("user1", new[] { "g1", "g2" }, new RolesEnum[] { RolesEnum.Cashier });

            UserContexts.User1 = new UserSecuritySchema<MyUserSecurityContext>(policyRepo, userSecurityContext1);
        }
    }
}
