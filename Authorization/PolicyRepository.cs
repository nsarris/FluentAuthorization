using FluentAuthorization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Authorization
{

    public class PolicyRepository: IPolicyRepository<MyUserSecurityContext>
    {
        readonly SecurityObjectPolicyRepository<string, MyUserSecurityContext> userPolicies;
        readonly SecurityObjectPolicyRepository<string, MyUserSecurityContext> groupPolicies;
        readonly SecurityObjectPolicyRepository<RolesEnum, MyUserSecurityContext> rolePolicies;
        
        public PolicyRepository(
            SecurityObjectPolicyRepository<string, MyUserSecurityContext> userPolicies,
            SecurityObjectPolicyRepository<string, MyUserSecurityContext> groupPolicies,
            SecurityObjectPolicyRepository<RolesEnum, MyUserSecurityContext> rolePolicies)
        {
            this.userPolicies = userPolicies;
            this.groupPolicies = groupPolicies;
            this.rolePolicies = rolePolicies;
        }

        public List<SecurityPolicy<MyUserSecurityContext>> GetByRole(RolesEnum role)
        {
            return rolePolicies[role].ToList();
        }

        public List<SecurityPolicy<MyUserSecurityContext>> GetByRole(Type policyType, RolesEnum role)
        {
            return rolePolicies[role].Where(x => x.GetType() == policyType).ToList();
        }

        public List<T> GetByRole<T>(RolesEnum role) where T : SecurityPolicy<MyUserSecurityContext>
        {
            return rolePolicies[role].OfType<T>().ToList();
        }

        public List<SecurityPolicy<MyUserSecurityContext>> GetByUser(string userId)
        {
            return userPolicies[userId].ToList();
        }

        public List<SecurityPolicy<MyUserSecurityContext>> GetByUser(string userId, IEnumerable<string> userGroupIds, IEnumerable<RolesEnum> roles)
        {
            var policies = userPolicies[userId];
            if (userGroupIds != null)
                foreach (var group in userGroupIds)
                    policies.Concat(groupPolicies[group]);
            if (roles != null)
                foreach (var role in roles)
                    policies.Concat(rolePolicies[role]);

            return policies.Distinct().ToList();
        }

        public List<SecurityPolicy<MyUserSecurityContext>> GetByUser(Type policyType, string userId)
        {
            return userPolicies[userId].Where(x => x.GetType() == policyType).ToList();
        }

        public List<SecurityPolicy<MyUserSecurityContext>> GetByUser(Type policyType, string userId, IEnumerable<string> userGroupIds, IEnumerable<RolesEnum> roles)
        {
            var policies = userPolicies[userId].Where(x => x.GetType() == policyType);
            if (userGroupIds != null)
                foreach (var group in userGroupIds)
                    policies.Concat(groupPolicies[group].Where(x => x.GetType() == policyType));
            if (roles != null)
                foreach (var role in roles)
                    policies.Concat(rolePolicies[role].Where(x => x.GetType() == policyType));

            return policies.Distinct().ToList();
        }

        public List<T> GetByUser<T>(string userId) where T : SecurityPolicy<MyUserSecurityContext>
        {
            return userPolicies[userId].OfType<T>().ToList();
        }

        public List<T> GetByUser<T>(string userId, IEnumerable<string> userGroupIds, IEnumerable<RolesEnum> roles) where T : SecurityPolicy<MyUserSecurityContext>
        {
            return GetByUser(typeof(T), userId, userGroupIds, roles).Cast<T>().ToList();
        }

        public List<SecurityPolicy<MyUserSecurityContext>> GetByUserGroup(string userGroupId)
        {
            return groupPolicies[userGroupId].ToList();
        }

        public List<SecurityPolicy<MyUserSecurityContext>> GetByUserGroup(Type policyType, string userGroupId)
        {
            return groupPolicies[userGroupId].Where(x => x.GetType() == policyType).ToList();
        }

        public List<T> GetByUserGroup<T>(string userGroupId) where T : SecurityPolicy<MyUserSecurityContext>
        {
            return groupPolicies[userGroupId].OfType<T>().ToList();
        }

        public List<SecurityPolicy<MyUserSecurityContext>> GetByUserSecurityContext(MyUserSecurityContext securityContext)
        {
            return GetByUser(securityContext.UserId, securityContext.GroupIds, securityContext.Roles);
        }

        public List<SecurityPolicy<MyUserSecurityContext>> GetByUserSecurityContext(Type policyType, MyUserSecurityContext securityContext)
        {
            return GetByUser(policyType, securityContext.UserId, securityContext.GroupIds, securityContext.Roles);
        }

        public List<T> GetByUserSecurityContext<T>(MyUserSecurityContext securityContext) where T : SecurityPolicy<MyUserSecurityContext>
        {
            return GetByUser(typeof(T), securityContext.UserId, securityContext.GroupIds, securityContext.Roles).Cast<T>().ToList();
        }
    }


}
