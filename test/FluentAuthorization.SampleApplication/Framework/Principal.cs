using FluentAuthorization;
using System.Collections.Generic;

namespace SampleApplication.Authorization
{
    public class Principal
    {
        public string UserId { get; }
        public string UserName { get; }

        public IEnumerable<string> GroupIds { get; }

        public IEnumerable<RolesEnum> Roles { get; }

        public Principal(string userid, string userName, IEnumerable<string> groups, IEnumerable<RolesEnum> roles)
        {
            UserId = userid;
            UserName = userName;
            GroupIds = groups;
            Roles = roles;
        }

        public override string ToString()
            => $"{UserName} ({UserId})";
    }
}
