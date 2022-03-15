using FluentAuthorization;
using System.Collections.Generic;

namespace SampleApplication.Authorization
{
    public class Principal
    {
        public string UserId { get; }
        public string UserName { get; }

        public IEnumerable<Roles> Roles { get; }

        public Principal(string userid, string userName, IEnumerable<Roles> roles)
        {
            UserId = userid;
            UserName = userName;
            Roles = roles;
        }

        public override string ToString()
            => $"{UserName} ({UserId})";
    }
}
