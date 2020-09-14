using System;
using Firebase.Auth;

namespace Plugin.FirebaseAuth
{
    public class PhoneMultiFactorGeneratorWrapper : IPhoneMultiFactorGenerator
    {
        public IPhoneMultiFactorAssertion GetAssertion(IPhoneAuthCredential phoneAuthCredential)
        {
            var assertion = PhoneMultiFactorGenerator.GetAssertion(phoneAuthCredential.ToNative());
            return new PhoneMultiFactorAssertionWrapper(assertion);
        }
    }
}
