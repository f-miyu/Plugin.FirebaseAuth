using System;
using Firebase.Auth;
namespace Plugin.FirebaseAuth
{
    public class PhoneAuthCredentialWrapper : IPhoneAuthCredential
    {
        internal PhoneAuthCredential PhoneAuthCredential { get; }

        public string Provider => PhoneAuthCredential.Provider;

        public PhoneAuthCredentialWrapper(PhoneAuthCredential phoneAuthCredential)
        {
            PhoneAuthCredential = phoneAuthCredential;
        }
    }
}
