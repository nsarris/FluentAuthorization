using System.Collections.Generic;

namespace FluentAuthorization
{
    public interface IPermissionReduceStrategy
    {
        AssertionResult Reduce(IEnumerable<AssertionResult> Permissions);
    }

}
