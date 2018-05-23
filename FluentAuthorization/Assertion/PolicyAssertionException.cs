using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentAuthorization
{
    class PolicyAssertionException : UnauthorizedAccessException
    {
        public IEnumerable<EffectivePermission> DeniedPermissions { get; }
        //public object ErrorCode { get; }
        //static readonly Lazy<string> defaultMessage = new Lazy<string>(() => new UnauthorizedAccessException().Message);

        public PolicyAssertionException(AssertionResult assertion)
            : base(assertion.DenialMessage) => DeniedPermissions = assertion.DeniedPermissions;
    }
}
