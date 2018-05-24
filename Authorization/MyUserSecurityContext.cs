using FluentAuthorization;
using System.Collections.Generic;

namespace Authorization
{
    public enum RolesEnum
    {
        Cashier,
        Officer,
        Manager,
        Head,
        Director,
        GeneralManager
    }

    public class MyUserSecurityContext
    {
        public string UserId => "xd89";

        public IEnumerable<string> GroupIds => new List<string> { "Group1", "Group2" };

        public IEnumerable<RolesEnum> Roles => new List<RolesEnum> {  };
    }



}
