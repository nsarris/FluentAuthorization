//using System;
//using System.Linq;
//using System.Collections.Generic;

//namespace PolicyAuthorization
//{

//    public sealed class PolicyAssertion : IPolicyAssertion
//    {
//        private bool not;
//        private UserSecuritySchema policyProvider;
//        private Func<AuthorizationError> errorBuilder;
        
//        private class PolicyAssertionNode
//        {
//            public Type SecurityPolicyType;
//            public Func<SecurityPolicy, bool> PolicyAssertionDelegate;
//            public LogicalOperator LogicalOperator;
//            public bool Not;
//        }

//        private List<PolicyAssertionNode> nodes = new List<PolicyAssertionNode>();
//        internal PolicyAssertion(UserSecuritySchema policyProvider, bool not = false)
//        {
//            this.policyProvider = policyProvider;
//            this.not = not;
//        }

//        private PolicyAssertion(PolicyAssertion copyFrom, PolicyAssertionNode newNode)
//        {
//            this.policyProvider = copyFrom.policyProvider;
//            this.not = copyFrom.not;
//            this.errorBuilder = copyFrom.errorBuilder;

//            var nodes = copyFrom.nodes.ToList();
            
//            if (newNode != null) nodes.Add(newNode);
//            this.nodes = nodes;
//        }

//        public IPolicyAssertion Not()
//        {
//            var p = new PolicyAssertion(this, null);
//            p.not = !p.not;
//            return p;
//        }

//        public IPolicyAssertion And<T>(Func<T, bool> assertion)
//            where T : SecurityPolicy
//        {
//            return new PolicyAssertion(
//                this,
//                new PolicyAssertionNode()
//                {
//                    SecurityPolicyType = typeof(T),
//                    PolicyAssertionDelegate = (SecurityPolicy x) => assertion((T)x),
//                    LogicalOperator = LogicalOperator.And,
//                });
//        }

//        public IPolicyAssertion AndNot<T>(Func<T, bool> assertion)
//            where T : SecurityPolicy
//        {
//            return new PolicyAssertion(
//               this,
//               new PolicyAssertionNode()
//               {
//                   SecurityPolicyType = typeof(T),
//                   PolicyAssertionDelegate = (SecurityPolicy x) => assertion((T)x),
//                   LogicalOperator = LogicalOperator.And,
//                   Not = true
//               });
//        }

//        public IPolicyAssertion Or<T>(Func<T, bool> assertion)
//            where T : SecurityPolicy
//        {
//            return new PolicyAssertion(
//               this,
//               new PolicyAssertionNode()
//               {
//                   SecurityPolicyType = typeof(T),
//                   PolicyAssertionDelegate = (SecurityPolicy x) => assertion((T)x),
//                   LogicalOperator = LogicalOperator.Or,
//               });
//        }

//        public IPolicyAssertion OrNot<T>(Func<T, bool> assertion)
//            where T : SecurityPolicy
//        {
//            return new PolicyAssertion(
//               this,
//               new PolicyAssertionNode()
//               {
//                   SecurityPolicyType = typeof(T),
//                   PolicyAssertionDelegate = (SecurityPolicy x) => assertion((T)x),
//                   LogicalOperator = LogicalOperator.Or,
//                   Not = true
//               });
//        }


//        public IPolicyAssertion And(Func<bool> assertion)
//        {
//            return new PolicyAssertion(
//               this,
//               new PolicyAssertionNode()
//               {
//                   PolicyAssertionDelegate = (SecurityPolicy x) => assertion(),
//                   LogicalOperator = LogicalOperator.And,
//               });
//        }

//        public IPolicyAssertion AndNot(Func<bool> assertion)
//        {
//            return new PolicyAssertion(
//               this,
//               new PolicyAssertionNode()
//               {
//                   PolicyAssertionDelegate = (SecurityPolicy x) => assertion(),
//                   LogicalOperator = LogicalOperator.And,
//                   Not = true
//               });
//        }

//        public IPolicyAssertion Or(Func<bool> assertion)
//        {
//            return new PolicyAssertion(
//               this,
//               new PolicyAssertionNode()
//               {
//                   PolicyAssertionDelegate = (SecurityPolicy x) => assertion(),
//                   LogicalOperator = LogicalOperator.Or,
//               });
//        }

//        public IPolicyAssertion OrNot(Func<bool> assertion)
//        {
//            return new PolicyAssertion(
//               this,
//               new PolicyAssertionNode()
//               {
//                   PolicyAssertionDelegate = (SecurityPolicy x) => assertion(),
//                   LogicalOperator = LogicalOperator.Or,
//                   Not = true
//               });
//        }



//        public IPolicyAssertion And(Func<IPolicyAssertion> assertion)
//        {
//            return new PolicyAssertion(
//               this,
//               new PolicyAssertionNode()
//               {
//                   PolicyAssertionDelegate = (SecurityPolicy x) => assertion().Assert(),
//                   LogicalOperator = LogicalOperator.And,
//               });
//        }

//        public IPolicyAssertion AndNot(Func<IPolicyAssertion> assertion)
//        {
//            return new PolicyAssertion(
//               this,
//               new PolicyAssertionNode()
//               {
//                   PolicyAssertionDelegate = (SecurityPolicy x) => assertion().Assert(),
//                   LogicalOperator = LogicalOperator.And,
//                   Not = true
//               });
//        }

//        public IPolicyAssertion Or(Func<IPolicyAssertion> assertion)
//        {
//            return new PolicyAssertion(
//               this,
//               new PolicyAssertionNode()
//               {
//                   PolicyAssertionDelegate = (SecurityPolicy x) => assertion().Assert(),
//                   LogicalOperator = LogicalOperator.Or,
//               });
//        }

//        public IPolicyAssertion OrNot(Func<IPolicyAssertion> assertion)
//        {
//            return new PolicyAssertion(
//              this,
//              new PolicyAssertionNode()
//              {
//                  PolicyAssertionDelegate = (SecurityPolicy x) => assertion().Assert(),
//                  LogicalOperator = LogicalOperator.Or,
//                  Not = true
//              });
//        }


//        public IPolicyAssertion And(Func<UserSecuritySchema, IPolicyAssertion> assertion)
//        {
//            return new PolicyAssertion(
//              this,
//              new PolicyAssertionNode()
//              {
//                  PolicyAssertionDelegate = (SecurityPolicy x) => assertion(policyProvider).Assert(),
//                  LogicalOperator = LogicalOperator.And,
//              });
//        }

//        public IPolicyAssertion AndNot(Func<UserSecuritySchema, IPolicyAssertion> assertion)
//        {
//            return new PolicyAssertion(
//              this,
//              new PolicyAssertionNode()
//              {
//                  PolicyAssertionDelegate = (SecurityPolicy x) => assertion(policyProvider).Assert(),
//                  LogicalOperator = LogicalOperator.And,
//                  Not = true
//              });
//        }

//        public IPolicyAssertion Or(Func<UserSecuritySchema, IPolicyAssertion> assertion)
//        {
//            return new PolicyAssertion(
//              this,
//              new PolicyAssertionNode()
//              {
//                  PolicyAssertionDelegate = (SecurityPolicy x) => assertion(policyProvider).Assert(),
//                  LogicalOperator = LogicalOperator.Or,
//              });
//        }

//        public IPolicyAssertion OrNot(Func<UserSecuritySchema, IPolicyAssertion> assertion)
//        {
//            return new PolicyAssertion(
//              this,
//              new PolicyAssertionNode()
//              {
//                  PolicyAssertionDelegate = (SecurityPolicy x) => assertion(policyProvider).Assert(),
//                  LogicalOperator = LogicalOperator.Or,
//                  Not = true
//              });
//        }

//        public bool Assert()
//        {
//            var result = true;

//            foreach (var node in nodes)
//            {
//                var policy =
//                    node.SecurityPolicyType != null ?
//                    policyProvider.GetPolicy(node.SecurityPolicyType) :
//                    null;

//                var policyResult = node.PolicyAssertionDelegate(policy);
//                if (node.Not) policyResult = !policyResult;

//                result = node.LogicalOperator == LogicalOperator.And ?
//                    result && policyResult :
//                    result || policyResult;
//            }

//            if (this.not) result = !result;

//            return result;
//        }

//        public void Throw()
//        {
//            if (!Assert())
//            {
//                if (errorBuilder != null)
//                {
//                    throw new PolicyAssertionException(errorBuilder.Invoke());
//                }
//                else
//                    throw new PolicyAssertionException();
//            }
//        }

//        public IPolicyAssertion WithError(Func<AuthorizationError> errorBuilder)
//        {
//            this.errorBuilder = errorBuilder;
//            return new PolicyAssertion(this, null);
//        }
//    }

//}
