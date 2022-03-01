using System;
using System.Linq;
using System.Collections.Generic;

namespace FluentAuthorization
{
    public sealed class AssertionContainer<TUserSecurityContext> : IAssertable
    {
        Func<string> errorBuilder;
        readonly UserSecuritySchema<TUserSecurityContext> policyProvider;
        readonly List<Func<UserSecuritySchema<TUserSecurityContext>, IAssertable>> permissionProducers = new List<Func<UserSecuritySchema<TUserSecurityContext>, IAssertable>>();
        readonly LogicalOperator logicalOperator;
        readonly IPermissionCalculationStrategy calculationStrategy;
        readonly TUserSecurityContext userSecurityContext;

        internal AssertionContainer(UserSecuritySchema<TUserSecurityContext> policyProvider, TUserSecurityContext userSecurityContext, LogicalOperator logicalOperator, IPermissionCalculationStrategy calculationStrategy)
        {
            this.policyProvider = policyProvider;
            this.logicalOperator = logicalOperator;
            this.calculationStrategy = calculationStrategy;
            this.userSecurityContext = userSecurityContext;
        }

        public AssertionContainer<TUserSecurityContext> Has<T>(Func<T, SecurityPolicy<TUserSecurityContext>.IPermission> getPermission)
            where T : SecurityPolicy<TUserSecurityContext>
        {
            permissionProducers.Add(_ =>
                new ResolvedPolicyAssertion(
                    calculationStrategy.Calculate(
                        policyProvider.GetPolicies<T>(), 
                        (T p) => getPermission(p).Assert(userSecurityContext))));
            return this;
        }

        public AssertionContainer<TUserSecurityContext> Has<T, TInput>(Func<T, SecurityPolicy<TUserSecurityContext>.IPermission<TInput>> getPermission, TInput input)
            where T : SecurityPolicy<TUserSecurityContext>
        {
            permissionProducers.Add(_ =>
                new ResolvedPolicyAssertion(
                    calculationStrategy.Calculate(
                        policyProvider.GetPolicies<T>(), 
                        (T p) => getPermission(p).Assert(userSecurityContext, input))));
            return this;
        }

        //Candidate - Return arbitrary result directly

        //public AssertionContainer<TUserSecurityContext> Has<T>(Func<T, AssertionResult> assertion)
        //    where T : SecurityPolicy<TUserSecurityContext>
        //{
        //    permissionProducers.Add(validator =>
        //        new ResolvedPolicyAssertion(calculationStrategy.Calculate(policyProvider.GetPolicies<T>(), p => assertion(p))));
        //    return this;
        //}
        //public AssertionContainer<TUserSecurityContext> Has<T, TInput>(Func<T, TInput, AssertionResult> assertion, TInput input)
        //    where T : SecurityPolicy<TUserSecurityContext>
        //{
        //    permissionProducers.Add(validator =>
        //        new ResolvedPolicyAssertion(calculationStrategy.Calculate(policyProvider.GetPolicies<T>(), p => assertion(p, input))));
        //    return this;
        //}

        public AssertionContainer<TUserSecurityContext> Has<T>(Func<T, Func<UserSecuritySchema<TUserSecurityContext>, IAssertable>> getAssertion)
            where T : SecurityPolicy<TUserSecurityContext>
        {
            permissionProducers.Add(schema =>
                new ResolvedPolicyAssertion(
                    calculationStrategy.Calculate(
                        policyProvider.GetPolicies<T>(),
                        p => getAssertion(p)(schema).Assert())));
            return this;
        }


        public AssertionContainer<TUserSecurityContext> Has<T, TInput>(Func<T, Func<UserSecuritySchema<TUserSecurityContext>, TInput, IAssertable>> getAssertion, TInput input)
            where T : SecurityPolicy<TUserSecurityContext>
        {
            permissionProducers.Add(schema =>
                new ResolvedPolicyAssertion(
                    calculationStrategy.Calculate(
                        policyProvider.GetPolicies<T>(), 
                        p => getAssertion(p)(schema, input).Assert())));
            return this;
        }
        public AssertionContainer<TUserSecurityContext> Has(Func<UserSecuritySchema<TUserSecurityContext>, IAssertable> assertion)
        {
            permissionProducers.Add(assertion);
            return this;
        }
        public AssertionContainer<TUserSecurityContext> WithError(Func<string> errorBuilder)
        {
            this.errorBuilder = errorBuilder;
            return this;
        }

        public AssertionResult Assert()
        {
            var permissions = permissionProducers.Select(x => x(policyProvider).Assert());
            var failedPermissions = permissions.Where(x => x.Deny).ToList();

            var success =
                logicalOperator == LogicalOperator.And ?
                !failedPermissions.Any() :
                failedPermissions.Count != permissionProducers.Count;

            var deniedPermissions = failedPermissions.SelectMany(x => x.DeniedPermissions);

            return new AssertionResult(success, deniedPermissions,
                errorBuilder?.Invoke()
                ??
                string.Join(Environment.NewLine, failedPermissions
                    .Select(x => x.DenialMessage)));

        }
    }
}
