using FluentAuthorization;
using SampleApplication.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleApplication.Authorization.Policies
{


    public class CustomerEntityPolicy : BasePolicy<EntityTypeResource, CustomerEntityPolicy.Data>
    {
        public class Data
        {
            public bool ViewVip { get; }
            public bool ViewPersonnel { get; }
            public bool View { get; }
            public bool Create { get; }
            public bool Update { get; }
            public bool Delete { get; }
            public decimal ViewBalanceLimit { get; }
            public bool ViewRealNames { get; }

            public Data(bool create, bool update, bool delete, bool view, bool viewVip, bool viewPersonnel, decimal viewBalanceLimit, bool viewRealNames)
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
        public Permission View { get; }
        public Permission Create { get; }
        public Permission Update { get; }
        public Permission Delete { get; }
        public Permission<Customer> ViewName { get; }
        public Permission<Customer> ViewCustomer { get; }

        public CustomerEntityPolicy() 
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
