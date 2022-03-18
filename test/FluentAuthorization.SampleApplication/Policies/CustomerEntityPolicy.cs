using FluentAuthorization;
using SampleApplication.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleApplication.Authorization.Policies
{
    public class CustomerEntityPolicy : BaseEntityPolicy<CustomerEntityPolicy.Data>
    {
        public class Data : BaseEntityPolicyData
        {
            public bool? ViewVip { get; init; }
            public bool? ViewPersonnel { get; init; }
            public decimal? ViewBalanceLimit { get; init; }
            public bool? ViewRealNames { get; init; }

            public Data(bool? create, bool? update, bool? delete, bool? view, bool? viewVip, bool? viewPersonnel, decimal? viewBalanceLimit, bool? viewRealNames)
                :base(create, update, delete, view)
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

        public Permission<Customer> ViewName { get; }
        public Permission<Customer> ViewCustomer { get; }

        public CustomerEntityPolicy()
        {
            ViewCustomer = permissionBuilder
                .AssertWith<Customer>(ctx =>
                {
                    if (ctx.User.Roles.Any(x => x == Roles.GeneralManager))
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
                        ctx.User.Roles.Any(x => x == Roles.GeneralManager) ||
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
