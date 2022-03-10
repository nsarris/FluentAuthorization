using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentAuthorization
{
    internal static class DefaultMessageBuilder
    {
        public static string BuildMessage(string user, string policy, string permission)
        {
            return $"User {user} was denied permission {permission} of policy {policy}.";
        }

        public static string BuildFailureMessage(string message, string reason)
        {
            return string.IsNullOrEmpty(reason) ? message : $"{message} - Reason: {reason}";
        }
    }

    public class AssertionFailure
    {
        public AssertionFailure(string user, string permissionName, string policyName, string message, string reason)
        {
            User = user;
            Permission = permissionName;
            Policy = policyName;
            Message = message;
            Reason = reason;
        }

        public string User { get; }
        public string Permission { get; }
        public string Policy { get; }
        public string Message { get; }
        public string Reason { get; }

        public override string ToString()
        {
            return DefaultMessageBuilder.BuildFailureMessage(Message, Reason);
        }
    }
    
    public class AssertionResult
    {
        public static AssertionResult Success { get; } = new();

        public AssertionResult()
        {
            Allow = true;
            Failures = Enumerable.Empty<AssertionFailure>();
        }

        public AssertionResult(AssertionFailure failure)
        {
            Allow = false;
            Failures = failure is null ? Enumerable.Empty<AssertionFailure>() : new[] { failure };
        }

        public AssertionResult(bool allow, IEnumerable<AssertionFailure> reasons)
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

        public static bool operator true(AssertionResult result)
        {
            return result?.Allow ?? true;
        }

        public static bool operator false(AssertionResult result)
        {
            return result?.Deny ?? false;
        }

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
