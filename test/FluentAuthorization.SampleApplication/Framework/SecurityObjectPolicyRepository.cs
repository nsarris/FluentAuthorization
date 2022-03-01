//using FluentAuthorization;
//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace SampleApplication.Authorization
//{

//    public class SecurityObjectPolicyRepository<TKey, TUserSecurityContext> 
//    {
//        readonly PolicyLookup<TKey, TUserSecurityContext> policies;


//        public SecurityObjectPolicyRepository(PolicyLookup<TKey, TUserSecurityContext> policies)
//        {
//            this.policies = policies;
//        }

//        public IEnumerable<SecurityPolicy<TUserSecurityContext>> GetByObjectId(TKey id)
//        {
//            return policies[id];
//        }

//        public IEnumerable<SecurityPolicy<TUserSecurityContext>> GetByObjectIdAndType(TKey id, Type policyType)
//        {
//            return policies[id].Where(x => x.GetType() == policyType);
//        }

//        public IEnumerable<T> GetByObjectIdAndType<T>(TKey id, Type policyType)
//            where T : SecurityPolicy<TUserSecurityContext>
//        {
//            return policies[id].OfType<T>();
//        }

//        public IEnumerable<SecurityPolicy<TUserSecurityContext>> this[TKey id]
//        {
//            get
//            {
//                return GetByObjectId(id);
//            }
//        }

//        public IEnumerable<SecurityPolicy<TUserSecurityContext>> this[TKey id, Type policyType]
//        {
//            get
//            {
//                return GetByObjectIdAndType(id, policyType);
//            }
//        }
//    }


//}

