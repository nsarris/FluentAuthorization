//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace PolicyAuthorization
//{
//    public class UserSecuritySchema : UserSecuritySchema
//    {
//        private readonly IUserSecurityContext securityContext;
//        private readonly List<SecurityPolicy> policies = new List<SecurityPolicy>();
//        private readonly IPolicyRepository policyRepository;
        
//        public UserSecuritySchema(IPolicyRepository policyRepository, IUserSecurityContext securityContext)
//        {
//            this.policyRepository = policyRepository;
//            this.securityContext = securityContext;
//        }

//        //public void AddPolicy(SecurityPolicy policy)
//        //{
//        //    if (policy == null) throw new ArgumentNullException(nameof(policy));

//        //    if (policies.Any(x => x.GetType() == policy.GetType()))
//        //        throw new Exception("Policy of type " + policy.GetType().Name + " already exists.");

//        //    policies.Add(policy);
//        //}



//        public T GetPolicy<T>()
//            where T : SecurityPolicy
//        {
//            var r = policies.OfType<T>().FirstOrDefault();
//            //if (r == null)
//              //  r = policyRepository.GetByUserSecurityContext<T>(securityContext);
            
//            policies.Add(r);
//            return r;
//        }



//        public SecurityPolicy GetPolicy(Type policyType)
//        {
//            if (!typeof(SecurityPolicy).IsAssignableFrom(policyType)) throw new ArgumentException("Policy type must be of SecurityPolicy type", nameof(policyType));

//            var r = policies.Where(x => x.GetType() == policyType).FirstOrDefault();
//            //if (r == null)
//              //  r = policyRepository.GetByUserSecurityContext(policyType, securityContext);
            
//            policies.Add(r);
//            return r;
//        }

//        //public bool AssertPolicy<T>(Func<T, bool> ruleExpression)
//        //    where T : SecurityPolicy
//        //{
//        //    return CheckPolicy(ruleExpression).Success;
//        //}

//        //public AuthorizationResult<T> CheckPolicy<T>(Func<T, bool> ruleExpression)
//        //    where T : SecurityPolicy
//        //{
//        //    T policy = null;

//        //    try
//        //    {
//        //        policy = GetPolicy<T>();
//        //        var success = ruleExpression == null ? true : ruleExpression(policy);

//        //        return new AuthorizationResult<T>()
//        //        {
//        //            Success = success,
//        //            Message = success ? "" : policy?.GetDefaultMessage() ?? "",
//        //        };


//        //        //audit/log
//        //    }
//        //    catch (Exception e)
//        //    {
//        //        return new AuthorizationResult<T>()
//        //        {
//        //            Message = "An error occured while checking policy " + (policy?.Name ?? typeof(T).Name) + " : " + e.Message,
//        //        };
//        //        //audit/log
//        //    }
//        //}

//        public IPolicyAssertion When<T>(Func<T, bool> assertion)
//            where T : SecurityPolicy
//        {
//            return new PolicyAssertion(this).And(assertion);
//        }

//        public IPolicyAssertion WhenNot<T>(Func<T, bool> assertion)
//            where T : SecurityPolicy
//        {
//            return new PolicyAssertion(this, true).And(assertion);
//        }
//    }
//}
