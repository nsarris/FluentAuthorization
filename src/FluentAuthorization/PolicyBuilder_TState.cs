//using System;

//namespace FluentAuthorization
//{
//    public class PolicyBuilder<TUser, TResource, T, TState>
//    {
//        public PolicyBuilder<TUser, TResource, T, TState> AssertWith(Func<Policy<TUser, TResource, T>.AssertionContext<TState>, AssertionResult> assert) { return this; }
//        public PolicyBuilder<TUser, TResource, T, TState> AssertWith(Func<Policy<TUser, TResource, T>.AssertionContext<TState>, bool?> assert) { return this; }
//        public PolicyBuilder<TUser, TResource, T, TState> AssertWith<TPermission>() where TPermission : Policy<TUser, TResource, T>.Permission<TState> { return this; }
//    }
//}
