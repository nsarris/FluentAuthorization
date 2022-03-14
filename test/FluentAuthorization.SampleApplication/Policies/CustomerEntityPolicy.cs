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
            public bool? ViewVip { get; }
            public bool? ViewPersonnel { get; }
            public bool? View { get; }
            public bool? Create { get; }
            public bool? Update { get; }
            public bool? Delete { get; }
            public decimal? ViewBalanceLimit { get; }
            public bool? ViewRealNames { get; }

            public Data(bool? create, bool? update, bool? delete, bool? view, bool? viewVip, bool? viewPersonnel, decimal? viewBalanceLimit, bool? viewRealNames)
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
                {
                    if (ctx.User.Roles.Any(x => x == RolesEnum.GeneralManager))
                        return ctx.Allow();

                    if (!ctx.Data.View.HasValue
                        && !ctx.Data.ViewPersonnel.HasValue
                        && !ctx.Data.ViewVip.HasValue)
                        return ctx.Undefined();

                    var reasons = new List<string>();

                    if (ctx.Data.View.IsConfigured(false))
                        reasons.Add("User does not have view access.");

                    if (ctx.Data.ViewPersonnel.IsConfigured(false)
                        && ctx.State.IsPersonnel)
                        reasons.Add("User does not have personnel access.");

                    if  (ctx.Data.ViewVip.IsConfigured(false)
                        && ctx.State.IsVip)
                        reasons.Add("User does not have vip access.");

                    if (ctx.Data.ViewBalanceLimit.HasValue
                        && ctx.State.Accounts.Sum(x => x.Balance) >= ctx.Data.ViewBalanceLimit)
                        reasons.Add($"Customer's total balance is larger than the users set limit of {ctx.Data.ViewBalanceLimit}");

                    if (reasons.Any())
                        return ctx.Deny(reasons);

                    return ctx.Allow();
                    
                })
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
