using System.Collections.Generic;
using System.Linq;

namespace FluentAuthorization
{
    public class AssertionResult
    {
        public AssertionResult(bool allow)
            :this(allow, (string)null)
        {

        }

        public AssertionResult(bool allow, string reason)
        {
            Allow = allow;
            Reasons = reason is null ? Enumerable.Empty<string>() : new[] { reason };
        }

        public AssertionResult(bool allow, IEnumerable<string> reasons)
        {
            Allow = allow;
            Reasons = reasons switch
            {
                null => Enumerable.Empty<string>(),
                ICollection<string> => reasons,
                _ => reasons.ToArray()
            };
        }

        public bool Allow { get; }
        public bool Deny => !Allow;

        public IEnumerable<string> Reasons { get; }

        public static bool operator true(AssertionResult result)
        {
            return result.Allow;
        }

        public static bool operator false(AssertionResult result)
        {
            return !result.Allow;
        }

        public static bool operator !(AssertionResult result)
        {
            return !result.Allow;
        }

        public static AssertionResult operator &(AssertionResult left, AssertionResult right)
        {
            return new AssertionResult(left.Allow && right.Allow, left.Reasons.Concat(right.Reasons));
        }

        public static AssertionResult operator |(AssertionResult left, AssertionResult right)
        {
            return new AssertionResult(left.Allow || right.Allow, left.Reasons.Concat(right.Reasons));
        }

        public static implicit operator bool(AssertionResult assertionResult)
        {
            return assertionResult.Allow;
        }

        public void ThrowOnDeny()
        {
            if (Deny)
                throw new PolicyAssertionException(this);
        }
    }
}
