using FluentAuthorization;
using System.Collections.Generic;

namespace Authorization
{
    
    
        public class MyUserSecurityContext 
        {
            public string UserId => "xd89";

            public IEnumerable<string> GroupIds => new List<string> { "Group1", "Group2" };

            public IEnumerable<string> RoleIds => new List<string> { "Role1", "Role2" };
        }


    
}
