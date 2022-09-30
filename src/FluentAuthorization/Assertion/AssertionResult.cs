using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentAuthorization
{
    /// <summary>
    /// The result of a policy permission assertion.
    /// </summary>
    public class AssertionResult
    {
        public static AssertionResult Success { get; } = new();

        private AssertionResult(bool deny)
        {
            Deny = deny;
            Allow = !deny;
        }

        public AssertionResult()
            :this(false)
        {
            Failures = Enumerable.Empty<AssertionFailure>();
        }

        public AssertionResult(IEnumerable<AssertionFailure> reasons)
            :this(true)
        {
            Failures = reasons switch
            {
                null => Enumerable.Empty<AssertionFailure>(),
                ICollection<AssertionFailure> => reasons,
                _ => reasons.ToArray()
            };
        }

        public AssertionResult(AssertionFailure failure)
            : this(new[] { failure })
        {

        }

        /// <summary>
        /// True if the permission is denied, false otherwise. This is always the opposite of Allow.
        /// </summary>
        public bool Deny { get; }
        
        /// <summary>
        /// True if the permission is allowed, false otherwise. This is always the opposite of Deny.
        /// </summary>
        public bool Allow { get; }

        /// <summary>
        /// The collection of failures if Deny is true, empty otherwise.
        /// </summary>
        public IEnumerable<AssertionFailure> Failures { get; }

        /// <summary>
        /// Asserts the result is definetly true for short circuit boolean logic.
        /// </summary>
        /// <param name="result">The given result.</param>
        /// <returns>True if the result is not null or Allow is true</returns>
        public static bool operator true(AssertionResult result) => result is not null && result.Allow ;

        /// <summary>
        /// Asserts the result is definetly false for short circuit boolean logic.
        /// </summary>
        /// <param name="result">The given result.</param>
        /// <returns>True if the result is not null or Deny is true</returns>
        public static bool operator false(AssertionResult result) => result is not null && result.Deny;
        
        /// <summary>
        /// Returns the logical negative of result. Defaults to deny if source is null.
        /// </summary>
        /// <param name="result">The given result.</param>
        /// <returns>True if result is null or Deny is True</returns>
        public static bool operator !(AssertionResult result)
        {
            return result?.Deny ?? false;
        }

        /// <summary>
        /// Performs a logical AND comparison between two results. Failures are appended if the result is Deny.
        /// </summary>
        /// <param name="left">The left result operand.</param>
        /// <param name="right">The right result operand.</param>
        /// <returns>The combined result.</returns>
        public static AssertionResult operator &(AssertionResult left, AssertionResult right)
        {
            if (left is null) return right;
            if (right is null) return left;
            
            return left.Allow && right.Allow ? 
                Success : 
                new AssertionResult(left.Failures.Concat(right.Failures));
        }

        /// <summary>
        /// Performs a logical OR comparison between two results. Failures are appended if the result is Deny.
        /// </summary>
        /// <param name="left">The left result operand.</param>
        /// <param name="right">The right result operand.</param>
        /// <returns>The combined result.</returns>
        public static AssertionResult operator |(AssertionResult left, AssertionResult right)
        {
            if (left is null) return right;
            if (right is null) return left;
            
            return left.Allow || right.Allow ? 
                Success :
                new AssertionResult(left.Failures.Concat(right.Failures));
        }

        /// <summary>
        /// Casts a result to boolean. Returns null if the result is not null, true if Allow, false otherwise.
        /// </summary>
        /// <param name="assertionresult">The result to cast.</param>
        public static implicit operator bool(AssertionResult assertionresult)
        {
            return assertionresult is not null && assertionresult.Allow;
        }

        /// <summary>
        /// Casts a result to nullable boolean. Returns null if the result is null, true if Allow, false otherwise.
        /// </summary>
        /// <param name="assertionresult">The result to cast.</param>
        public static implicit operator bool?(AssertionResult assertionresult)
        {
            return assertionresult?.Allow;
        }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, Failures.Select(x => x.Message.ToString()));
        }

        /// <summary>
        /// Throws an exception if the result is Deny.
        /// </summary>
        /// <exception cref="PolicyAssertionException">An exception based on the failures of the result.</exception>
        public void ThrowOnDeny()
        {
            if (Deny)
                throw new PolicyAssertionException(this);
        }
    }
}
