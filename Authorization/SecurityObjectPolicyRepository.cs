using FluentAuthorization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Authorization
{

    public class SecurityObjectPolicyRepository<TUserSecurityContext> 
    {
        readonly PolicyLookup<TUserSecurityContext> policies;


        public SecurityObjectPolicyRepository(PolicyLookup<TUserSecurityContext> policies)
        {
            this.policies = policies;
        }

        public IEnumerable<SecurityPolicy<TUserSecurityContext>> GetByObjectId(string id)
        {
            return policies[id];
        }

        public IEnumerable<SecurityPolicy<TUserSecurityContext>> GetByObjectIdAndType(string id, Type policyType)
        {
            return policies[id].Where(x => x.GetType() == policyType);
        }

        public IEnumerable<T> GetByObjectIdAndType<T>(string id, Type policyType)
            where T : SecurityPolicy<TUserSecurityContext>
        {
            return policies[id].OfType<T>();
        }

        public IEnumerable<SecurityPolicy<TUserSecurityContext>> this[string id]
        {
            get
            {
                return GetByObjectId(id);
            }
        }

        public IEnumerable<SecurityPolicy<TUserSecurityContext>> this[string id, Type policyType]
        {
            get
            {
                return GetByObjectIdAndType(id, policyType);
            }
        }
    }


}

