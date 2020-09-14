using System;
using Firebase.Auth;

namespace Plugin.FirebaseAuth
{
    public partial interface IPhoneAuthCredential
    {
        internal new PhoneAuthCredential ToNative();
    }
}
