using Authorization.Model;
using FluentAuthorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authorization.Policies
{
    
    public class CustomerPolicy : MyPolicy<CustomerPolicy.CustomerPolicyData>
    {
        public class CustomerPolicyData : PolicyData
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
                ViewVip = ViewVip;
                ViewPersonnel = viewPersonnel;
                ViewBalanceLimit = viewBalanceLimit;
                ViewRealNames = viewRealNames;
            }
        }

        public IPermission View { get; private set; }
        public IPermission Create { get; private set; }
        public IPermission Update { get; private set; }
        public IPermission Delete { get; private set; }
        public IPermission<Customer> ViewRealName { get; private set; }
        public IPermission<Customer> ViewCustomer { get; private set; }

        public CustomerPolicy(CustomerPolicyData data) : base(data)
        {
            var permissionBuilder = PermissionBuilderFactory.Get(this);

            View = permissionBuilder
                .For(x => x.View)
                .Assert((p, u) => p.View)
                .BuildAndSet();

            Create = permissionBuilder
                .For(x => x.Create)
                .Assert((p, u) => p.Create)
                .BuildAndSet();

            Update = permissionBuilder
                .For(x => x.Update)
                .Assert((p, u) => p.Update)
                .BuildAndSet();

            Delete = permissionBuilder
                .For(x => x.Delete)
                .Assert((p, u) => p.Delete)
                .BuildAndSet();


            ViewCustomer = permissionBuilder
                .For(x => x.ViewCustomer)
                .Assert((p, u, c) =>
                        u.Roles.Any(x => x == RolesEnum.GeneralManager) ||
                            (p.View &&
                            (p.ViewPersonnel || !c.IsPersonnel) &&
                            (p.ViewVip || !c.IsVip) &&
                            c.Accounts.Sum(x => x.Balance) <= p.ViewBalanceLimit)
                )
                .BuildAndSet();

            ViewRealName = permissionBuilder
                .For(x => x.ViewCustomer)
                .Assert((p, u, c) =>
                        u.Roles.Any(x => x == RolesEnum.GeneralManager) ||
                            (!c.IsVip &&
                            c.Accounts.Sum(x => x.Balance) <= p.ViewBalanceLimit)
                )
                .WithMessageBuilder((p, u, c) =>
                    $"Cannot view real name of VIP customer {c.Id}")
                .BuildAndSet();
        }
    }
}
