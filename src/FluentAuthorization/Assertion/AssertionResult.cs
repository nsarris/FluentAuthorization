using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentAuthorization
{
    public class AssertionResult
    {
        public static AssertionResult Success { get; } = new();

        internal AssertionResult()
        {
            Deny = false;
            Allow = !Deny;
            Failures = Enumerable.Empty<AssertionFailure>();
        }

        internal AssertionResult(IEnumerable<AssertionFailure> reasons)
        {
            Deny = true;
            Allow = !Deny;
            Failures = reasons switch
            {
                null => Enumerable.Empty<AssertionFailure>(),
                ICollection<AssertionFailure> => reasons,
                _ => reasons.ToArray()
            };
        }
        internal AssertionResult(AssertionFailure failure)
            : this(new[] { failure })
        {

        }

        public bool Deny { get; }
        public bool Allow { get; }

        public IEnumerable<AssertionFailure> Failures { get; }

        /// <summary>
        /// Switch off short-circuit behaviour to evaluate all assertions.
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool operator true(AssertionResult result) => result is not null && result.Allow ;

        /// <summary>
        /// Switch off short-circuit behaviour to evaluate all assertions.
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool operator false(AssertionResult result) => result is not null && result.Deny;
        

        public static bool operator !(AssertionResult result)
        {
            return result?.Deny ?? false;
        }

        public static AssertionResult operator &(AssertionResult left, AssertionResult right)
        {
            if (left is null) return right;
            if (right is null) return left;
            
            return left.Allow && right.Allow ? 
                Success : 
                new AssertionResult(left.Failures.Concat(right.Failures));
        }

        public static AssertionResult operator |(AssertionResult left, AssertionResult right)
        {
            if (left is null) return right;
            if (right is null) return left;
            
            return left.Allow || right.Allow ? 
                Success :
                new AssertionResult(left.Failures.Concat(right.Failures));
        }

        public static implicit operator bool(AssertionResult assertionresult)
        {
            return assertionresult is null || assertionresult.Allow;
        }

        public static implicit operator bool?(AssertionResult assertionresult)
        {
            return assertionresult?.Allow;
        }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, Failures.Select(x => x.Message.ToString()));
        }

        public void ThrowOnDeny()
        {
            if (Deny)
                throw new PolicyAssertionException(this);
        }
    }
}
