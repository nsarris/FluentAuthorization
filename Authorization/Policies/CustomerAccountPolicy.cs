using Authorization.Model;
using FluentAuthorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authorization.Policies
{
    public class CustomerAccountPolicy : MyPolicy<CustomerAccountPolicy.CustomerAccountPolicyData>
    {
        public class CustomerAccountPolicyData : PolicyData
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

        public IPermission View { get; private set; }
        public IPermission Add { get; private set; }
        public IPermission Update { get; private set; }
        public IPermission Remove { get; private set; }
        public IPermission<Account> ViewAccount { get; private set; }

        public CustomerAccountPolicy(CustomerAccountPolicyData data) : base(data)
        {
            var permissionBuilder = PermissionBuilderFactory.Get(this);

            View = permissionBuilder
                .For(x => x.View)
                .Assert((p, u) => p.View)
                .BuildAndSet();

            Add = permissionBuilder
                .For(x => x.Add)
                .Assert((p, u) => p.Add)
                .BuildAndSet();

            Update = permissionBuilder
                .For(x => x.Update)
                .Assert((p, u) => p.Update)
                .BuildAndSet();

            Remove = permissionBuilder
                .For(x => x.Remove)
                .Assert((p, u) => p.Remove)
                .BuildAndSet();

            ViewAccount = permissionBuilder
                .For(x => x.ViewAccount)
                .Assert((p, u, a) =>
                        u.Roles.Any(x => x == RolesEnum.GeneralManager) ||
                            (p.View &&
                            a.Balance <= p.ViewBalanceLimit)
                            )
                .BuildAndSet();
        }
    }
}
