using FluentAuthorization;
using System.Collections.Generic;

namespace SampleApplication.Authorization
{

    public class MyUser
    {
        public string UserId { get; }

        public IEnumerable<string> GroupIds { get; }

        public IEnumerable<RolesEnum> Roles { get; }

        public MyUser(string username, IEnumerable<string> groups, IEnumerable<RolesEnum> roles)
        {
            UserId = username;
            GroupIds = groups;
            Roles = roles;
        }
    }
}
