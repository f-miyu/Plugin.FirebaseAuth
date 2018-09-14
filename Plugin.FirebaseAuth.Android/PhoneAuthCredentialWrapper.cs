using System;
using Firebase.Auth;
namespace Plugin.FirebaseAuth
{
    public class PhoneAuthCredentialWrapper : AuthCredentialWrapper, IPhoneAuthCredential
    {
        internal PhoneAuthCredential PhoneAuthCredential { get; }

        public PhoneAuthCredentialWrapper(PhoneAuthCredential phoneAuthCredential) : base(phoneAuthCredential)
        {
            PhoneAuthCredential = phoneAuthCredential;
        }
    }
}
