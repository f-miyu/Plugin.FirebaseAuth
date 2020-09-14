using System;
using Firebase.Auth;

namespace Plugin.FirebaseAuth
{
    public partial interface IPhoneMultiFactorInfo
    {
        internal new PhoneMultiFactorInfo ToNative();
    }
}
