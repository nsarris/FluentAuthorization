using FluentAuthorization;
using System.Collections.Generic;

namespace SampleApplication.Authorization
{

    public class MyUserSecurityContext
    {
        public string UserId { get; }

        public IEnumerable<string> GroupIds { get; }

        public IEnumerable<RolesEnum> Roles { get; }

        public MyUserSecurityContext(string username, IEnumerable<string> groups, IEnumerable<RolesEnum> roles)
        {
            UserId = username;
            GroupIds = groups;
            Roles = roles;
        }
    }
}
