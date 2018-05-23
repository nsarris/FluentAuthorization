using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentAuthorization
{
    public interface IPolicyFactory
    {
        T Create<T>() where T : ISecurityPolicy;
        //T Create<T>(object data) where T : ISecurityPolicyWithData;
        ISecurityPolicyWithData Create(Type policyType, object data);
        //T Create<T,TData>(TData data) where T : ISecurityPolicy<TData> where TData : IPolicyData;
    }
}
