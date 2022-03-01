using System;

namespace FluentAuthorization
{
    public class PolicyBuilder<TUser, TResource, T>
    {
        public PolicyBuilder<TUser, TResource, T> AssertWith(Func<Policy<TUser, TResource, T>.AssertionContext, AssertionResult> assert) { return this; }
        public PolicyBuilder<TUser, TResource, T> AssertWith(Func<Policy<TUser, TResource, T>.AssertionContext, bool?> assert) { return this; }
        public PolicyBuilder<TUser, TResource, T> AssertWith<TPermission>() where TPermission : Policy<TUser, TResource, T>.Permission { return this; }
    }
}
