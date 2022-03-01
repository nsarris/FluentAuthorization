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
            public bool View { get; private set; }
            public bool Add { get; private set; }
            public bool Update { get; private set; }
            public bool Remove { get; private set; }
            public decimal ViewBalanceLimit { get; private set; }
            
            public CustomerAccountPolicyData(bool view, bool add, bool update, bool remove, decimal viewBalanceLimit)
            {

            }
        }

        public Permission View { get; private set; }
        public Permission Add { get; private set; }
        public Permission Update { get; private set; }
        public Permission Remove { get; private set; }
        public Permission<Account> ViewAccount { get; private set; }

        public CustomerAccountPolicy(CustomerAccountPolicyData data) 
        {
            this
                .For(x => x.View)
                .AssertWith(ctx => ctx.Data.View)
                ;

            this
                .For(x => x.Add)
                .AssertWith(ctx => ctx.Data.Add)
                ;

            this
                .For(x => x.Update)
                .AssertWith(ctx => ctx.Data.Update)
                ;

            this
                .For(x => x.Remove)
                .AssertWith(ctx => ctx.Data.Remove)
                ;

            this
                .For(x => x.ViewAccount)
                .AssertWith(ctx =>
                        ctx.User.Roles.Any(x => x == RolesEnum.GeneralManager) ||
                            (ctx.Data.View &&
                            ctx.State.Balance <= ctx.Data.ViewBalanceLimit)
                            )
                ;
        }
    }
}
