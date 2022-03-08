using FluentAuthorization;
using System.Collections.Generic;

namespace SampleApplication.Authorization
{

    public class Principal
    {
        public string UserId { get; }

        public IEnumerable<string> GroupIds { get; }

        public IEnumerable<RolesEnum> Roles { get; }

        public Principal(string username, IEnumerable<string> groups, IEnumerable<RolesEnum> roles)
        {
            UserId = username;
            GroupIds = groups;
            Roles = roles;
        }
    }
}
