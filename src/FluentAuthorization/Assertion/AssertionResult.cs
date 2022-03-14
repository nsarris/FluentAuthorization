﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentAuthorization
{
    public class AssertionResult
    {
        public static AssertionResult Success { get; } = new();

        internal AssertionResult()
        {
            Allow = true;
            Failures = Enumerable.Empty<AssertionFailure>();
        }

        internal AssertionResult(AssertionFailure failure)
        {
            Allow = false;
            Failures = failure is null ? Enumerable.Empty<AssertionFailure>() : new[] { failure };
        }

        internal AssertionResult(bool allow, IEnumerable<AssertionFailure> reasons)
        {
            Allow = allow;
            Failures = reasons switch
            {
                null => Enumerable.Empty<AssertionFailure>(),
                ICollection<AssertionFailure> => reasons,
                _ => reasons.ToArray()
            };
        }

        public bool Allow { get; }
        public bool Deny => !Allow;

        public IEnumerable<AssertionFailure> Failures { get; }

        /// <summary>
        /// Switch off short-circuit behaviour to evaluate all assertions.
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool operator true(AssertionResult result) => false;

        /// <summary>
        /// Switch off short-circuit behaviour to evaluate all assertions.
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool operator false(AssertionResult result) => false;
        

        public static bool operator !(AssertionResult result)
        {
            return result?.Deny ?? false;
        }

        public static AssertionResult operator &(AssertionResult left, AssertionResult right)
        {
            if (left is null) return right;
            if (right is null) return left;
            return new AssertionResult(left.Allow && right.Allow, left.Failures.Concat(right.Failures));
        }

        public static AssertionResult operator |(AssertionResult left, AssertionResult right)
        {
            if (left is null) return right;
            if (right is null) return left;
            return new AssertionResult(left.Allow || right.Allow, left.Failures.Concat(right.Failures));
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