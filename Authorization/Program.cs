using FluentAuthorization;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Authorization
{
    partial class Program
    {
        static void Main(string[] args)
        {
            var userPolicies = new PolicyLookup<MyUserSecurityContext>()
            {
                ("xd89", new ValueProcessing(new ValueProcessing.ValueProcessingData())),
                ("xd89", new AccessData(new AccessDataData())),
            };

            var groupPolicies = new PolicyLookup<MyUserSecurityContext>();
            var rolePolicies = new PolicyLookup<MyUserSecurityContext>();

            //var policyFactory = new MyPolicyFactory();

            var userPolicyRepo = new SecurityObjectPolicyRepository<MyUserSecurityContext>(userPolicies);
            var groupPolicyRepo = new SecurityObjectPolicyRepository<MyUserSecurityContext>(groupPolicies);
            var rolePolicyRepo = new SecurityObjectPolicyRepository<MyUserSecurityContext>(rolePolicies);
            //    new MyPolicyFactory(),
            //    new Lookup<string, SecurityPolicy>() { "test",new ValueProcessing() })

            var policyRepo = new PolicyRepository(userPolicyRepo, groupPolicyRepo, rolePolicyRepo);

            var userContext = new MyUserSecurityContext();
            //var securityContext = new UserSecuritySchema(policyRepo, userContext);

            //var r1 = securityContext.When<ValueProcessing>(x => x.AllowView()).Assert();

            ////securityContext.When<CombinedValuePolicy>(x => x.Allow(new Value())).Assert();

            var v = new UserSecuritySchema<MyUserSecurityContext>(policyRepo, userContext);

            var rr = v.WhenAll(a => a
                .Has<ValueProcessing>(x => x.AllowView)
                .Has((ValueProcessing x) => x.CombinedPermission)
                //.Has<ValueProcessing>(x => x.AllowView2())
                //.Has<ValueProcessing>(x => x.AllowView2(new Value()))
                .Has(x => x
                    .WhenAny(a1 => a1
                        .Has((AccessData y) => y.Allow)))
                .WithError(() => "Error1 Inner")
            )
            .AndAny(a => a
                .Has((AccessData x) => x.Allow)
                .WithError(() => "Error2 Inner")
                )
            .AndAll(a => 
                a.Has((CombinedPolicy x) => x.Allow)
                )
            //.WithError(() => "Error")
            .Assert()
            ;
        }

    }
}
