using FluentAuthorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authorization
{
    public class PolicyLookup<TKey, TUserSecurityContext> : MutableLookup<TKey, SecurityPolicy<TUserSecurityContext>>
    {
    }
}
