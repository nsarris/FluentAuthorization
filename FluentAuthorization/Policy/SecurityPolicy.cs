using System;
using System.Linq.Expressions;
using System.Reflection;

namespace FluentAuthorization
{

    public abstract partial class SecurityPolicy<TUserSecurityContext> : ISecurityPolicy
    {
        public string Name { get; protected set; }
        public string Source { get; protected set; }

        public SecurityPolicy()
        {
            Name = this.GetType().GetCustomAttribute<PolicyNameAttribute>()?.Name
                ?? this.GetType().Name;
        }

        public virtual ISecurityPolicy Merge(ISecurityPolicy next)
        {
            if (next == null) throw new ArgumentNullException("next");
            if (next.GetType() != this.GetType()) throw new ArgumentException("Merging of policies of different types not allowed");
            return next;
        }
    }

    public abstract partial class SecurityPolicy<T, TUserSecurityContext>
        : SecurityPolicy<TUserSecurityContext>, ISecurityPolicy<T>
        where T : PolicyData
    {
        public T Data { get; }

        PolicyData ISecurityPolicyWithData.Data => Data;

        public SecurityPolicy(T data)
            : base()
        {
            Data = data;
        }

        public override ISecurityPolicy Merge(ISecurityPolicy next)
        {
            if (next == null) throw new ArgumentNullException("next");
            if (next.GetType() != this.GetType()) throw new ArgumentException("Merging of policies of different types not allowed");

            var thisData = this.Data;
            var nextData = ((SecurityPolicy<T, TUserSecurityContext>)next).Data;
            
            return CreateNew(nextData);
        }

        protected virtual SecurityPolicy<T, TUserSecurityContext> CreateNew(T data)
        {
            return (SecurityPolicy<T, TUserSecurityContext>)Activator.CreateInstance(this.GetType(), new object[] { data });
        }
    }




}
