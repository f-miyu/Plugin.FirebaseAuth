using System;
namespace Plugin.FirebaseAuth
{
    public interface IPhoneMultiFactorGenerator
    {
        IPhoneMultiFactorAssertion GetAssertion(IPhoneAuthCredential phoneAuthCredential);
    }
}
