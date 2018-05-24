using Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentAuthorization.Tests
{
    public static class UserContexts
    {
        public static UserSecuritySchema<MyUserSecurityContext> User1 { get; set; }
    }
}
