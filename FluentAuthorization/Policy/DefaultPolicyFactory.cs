using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentAuthorization
{
    internal class DefaultPolicyFactory : IPolicyFactory
    {
        readonly static Lazy<DefaultPolicyFactory> instance = new Lazy<DefaultPolicyFactory>(() => new DefaultPolicyFactory());
        internal static DefaultPolicyFactory Instance => instance.Value;

        public T Create<T>()
            where T : ISecurityPolicy
        {
            return Activator.CreateInstance<T>();
        }

        public ISecurityPolicyWithData Create(Type policyType, object data)
        {
            return (ISecurityPolicyWithData)Activator.CreateInstance(policyType, new object[] { data });
        }
    }
}
