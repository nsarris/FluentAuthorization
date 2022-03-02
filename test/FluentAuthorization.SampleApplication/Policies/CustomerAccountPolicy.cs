using FluentAuthorization;
using SampleApplication.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleApplication.Authorization
{
    public class CustomerAccountPolicy : BasePolicy<EntityTypeResource, CustomerAccountPolicy.CustomerAccountPolicyData>
    {
        public class CustomerAccountPolicyData 
        {
            public CustomerAccountPolicyData(bool view, bool add, bool update, bool remove, decimal viewBalanceLimit)
            {
                View = view;
                Add = add;
                Update = update;
                Remove = remove;
                ViewBalanceLimit = viewBalanceLimit;
            }

            public bool View { get; }
            public bool Add { get; }
            public bool Update { get; }
            public bool Remove { get; }
            public decimal ViewBalanceLimit { get; }
        }

        public Permission View { get; private set; }
        public Permission Add { get; private set; }
        public Permission Update { get; private set; }
        public Permission Remove { get; private set; }
        public Permission<Account> ViewAccount { get; private set; }

        public CustomerAccountPolicy() 
        {
            View = policyBuilder
                .AssertWith(ctx => ctx.Data.View)
                .Build()
                ;

            Add = policyBuilder
                .AssertWith(ctx => ctx.Data.Add)
                .Build()
                ;

            Update = policyBuilder
                .AssertWith(ctx => ctx.Data.Update)
                .Build()
                ;

            Remove = policyBuilder
                .AssertWith(ctx => ctx.Data.Remove)
                .Build();
                ;

            ViewAccount = policyBuilder
                .AssertWith<Account>(ctx =>
                        ctx.User.Roles.Any(x => x == RolesEnum.GeneralManager) ||
                            (ctx.Data.View &&
                            ctx.State.Balance <= ctx.Data.ViewBalanceLimit)
                            )
                .Build()
                ;
        }
    }
}
