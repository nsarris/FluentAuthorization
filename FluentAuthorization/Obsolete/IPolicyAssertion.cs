//using System;

//namespace PolicyAuthorization
//{
//    public interface IPolicyAssertion
//    {
//        IPolicyAssertion Not();

//           IPolicyAssertion And<T>(Func<T, bool> assertion)
//           where T : SecurityPolicy;
//        IPolicyAssertion AndNot<T>(Func<T, bool> assertion)
//            where T : SecurityPolicy;
//        IPolicyAssertion Or<T>(Func<T, bool> assertion)
//            where T : SecurityPolicy;
//        IPolicyAssertion OrNot<T>(Func<T, bool> assertion)
//            where T : SecurityPolicy;

//        IPolicyAssertion And(Func<bool> assertion);
//        IPolicyAssertion AndNot(Func<bool> assertion);
//        IPolicyAssertion Or(Func<bool> assertion);
//        IPolicyAssertion OrNot(Func<bool> assertion);


//        IPolicyAssertion And(Func<IPolicyAssertion> assertion);
//        IPolicyAssertion AndNot(Func<IPolicyAssertion> assertion);
//        IPolicyAssertion Or(Func<IPolicyAssertion> assertion);
//        IPolicyAssertion OrNot(Func<IPolicyAssertion> assertion);

//        IPolicyAssertion And(Func<UserSecuritySchema, IPolicyAssertion> assertion);
//        IPolicyAssertion AndNot(Func<UserSecuritySchema, IPolicyAssertion> assertion);
//        IPolicyAssertion Or(Func<UserSecuritySchema, IPolicyAssertion> assertion);
//        IPolicyAssertion OrNot(Func<UserSecuritySchema, IPolicyAssertion> assertion);

//        IPolicyAssertion WithError(Func<AuthorizationError> errorBuilder);

//        bool Assert();
//        void Throw();
//    }
//}
