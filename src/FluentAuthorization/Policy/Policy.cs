using System;
using System.Collections.Generic;

namespace FluentAuthorization
{
    public abstract partial class Policy<TUser, TResource, T> : IPolicy<TUser, TResource, T>
    {
        Type IPolicy.ResourceType => typeof(TResource);
        Type IPolicy.DataType => typeof(T);
        Type IPolicy.UserType => typeof(TUser);

        public virtual string Key => this.GetType().FullName;
        public virtual string Name => this.GetType().Name;

        public override string ToString() => Name;

        private static string BuildDefaultMessage(AssertionContextBase context)
        {
            return DefaultMessageBuilder.BuildMessage(context.User.ToString(), context.PolicyName, context.PermissionName);
        }

        public abstract T Aggregate(IEnumerable<T> data);

        protected readonly PolicyBuilderRoot policyBuilder = new();

        protected Policy()
        {

        }
    }
}
