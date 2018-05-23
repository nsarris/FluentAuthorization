//using System;

//namespace PolicyAuthorization
//{
//    public interface UserSecuritySchema
//    {
//        T GetPolicy<T>() where T : SecurityPolicy;
//        SecurityPolicy GetPolicy(Type policyType);
//        IPolicyAssertion When<T>(Func<T, bool> assertion) where T : SecurityPolicy;
//        IPolicyAssertion WhenNot<T>(Func<T, bool> assertion) where T : SecurityPolicy;
//    }
//}
