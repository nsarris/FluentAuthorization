using FluentAuthorization;
using SampleApplication.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleApplication.Authorization.Policies
{
    
    public class CustomerPolicy : BasePolicy<EntityTypeResource, CustomerPolicy.CustomerPolicyData>
    {
        public class CustomerPolicyData
        {
            public bool ViewVip { get; private set; }
            public bool ViewPersonnel { get; private set; }
            public bool View { get; private set; }
            public bool Create { get; private set; }
            public bool Update { get; private set; }
            public bool Delete { get; private set; }
            public decimal ViewBalanceLimit { get; private set; }
            public bool ViewRealNames { get; private set; }

            public CustomerPolicyData(bool create, bool update, bool delete, bool view, bool viewVip, bool viewPersonnel, decimal viewBalanceLimit, bool viewRealNames)
            {
                Create = create;
                Update = update;
                Delete = delete;
                View = view;
                ViewVip = viewVip;
                ViewPersonnel = viewPersonnel;
                ViewBalanceLimit = viewBalanceLimit;
                ViewRealNames = viewRealNames;
            }
        }

        //[DisplayName]
        public Permission View { get; private set; }
        public Permission Create { get; private set; }
        public Permission Update { get; private set; }
        public Permission Delete { get; private set; }
        public Permission<Customer> ViewRealName { get; private set; }
        public Permission<Customer> ViewCustomer { get; private set; }

        public CustomerPolicy() 
        {
            View = policyBuilder
                .AssertWith(ctx => ctx.Data.View)
                .Build()
                ;

            this
                .For(x => x.Create)
                .AssertWith(ctx => ctx.Data.Create)
                //.Build()
                ;

            this
                .For(x => x.Update)
                .AssertWith(ctx => ctx.Data.Update)
                ;

            this
                .For(x => x.Delete)
                .AssertWith(ctx => ctx.Data.Delete)
                ;


            this
                .For(x => x.ViewCustomer)
                .AssertWith(ctx =>
                        ctx.User.Roles.Any(x => x == RolesEnum.GeneralManager) ||
                            (ctx.Data.View &&
                            (ctx.Data.ViewPersonnel || !ctx.State.IsPersonnel) &&
                            (ctx.Data.ViewVip || !ctx.State.IsVip) &&
                            ctx.State.Accounts.Sum(x => x.Balance) <= ctx.Data.ViewBalanceLimit)
                )
                ;

            this
                .For(x => x.ViewCustomer)
                .AssertWith(ctx =>
                        ctx.User.Roles.Any(x => x == RolesEnum.GeneralManager) ||
                            (!ctx.State.IsVip &&
                            ctx.State.Accounts.Sum(x => x.Balance) <= ctx.Data.ViewBalanceLimit)
                )
                //.WithMessageBuilder((p, u, c) =>
                //    $"Cannot view real name of VIP customer {c.Id}")
                //.BuildAndSet()
                ;
        }
    }
}
