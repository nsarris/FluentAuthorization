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
        public Permission<Customer> ViewName { get; private set; }
        public Permission<Customer> ViewCustomer { get; private set; }

        public CustomerPolicy() 
        {
            View = permissionBuilder
                .AssertWith(ctx => ctx.Data.View)
                .WithName(nameof(View))
                .Build()
                ;

            Create = permissionBuilder
                .AssertWith(ctx => ctx.Data.Create)
                .WithName(nameof(Create))
                .Build()
                ;

            Update = permissionBuilder
                .AssertWith(ctx => ctx.Data.Update)
                .WithName(nameof(Update))
                .Build()
                ;

            Delete = permissionBuilder
                .AssertWith(ctx => ctx.Data.Delete)
                .WithName(nameof(Delete))
                .Build() 
                ;

            ViewCustomer = permissionBuilder
                .AssertWith<Customer>(ctx =>
                        ctx.User.Roles.Any(x => x == RolesEnum.GeneralManager) ||
                            (ctx.Data.View &&
                            (ctx.Data.ViewPersonnel || !ctx.State.IsPersonnel) &&
                            (ctx.Data.ViewVip || !ctx.State.IsVip) &&
                            ctx.State.Accounts.Sum(x => x.Balance) <= ctx.Data.ViewBalanceLimit)
                )
                .WithName(nameof(ViewCustomer))
                .Build()
                ;

            ViewName = permissionBuilder
                .AssertWith<Customer>(ctx =>
                        ctx.User.Roles.Any(x => x == RolesEnum.GeneralManager) ||
                            (!ctx.State.IsVip &&
                            ctx.State.Accounts.Sum(x => x.Balance) <= ctx.Data.ViewBalanceLimit)
                )
                .WithName(nameof(ViewName))
                .WithMessage(ctx =>
                    $"Cannot view real name of VIP customer {ctx.State.Id}.")
                .Build()
                ;
        }
    }
}
