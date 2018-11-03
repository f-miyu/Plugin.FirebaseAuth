using System;
using Firebase.Auth;
namespace Plugin.FirebaseAuth
{
    public class PhoneAuthCredentialWrapper : AuthCredentialWrapper, IPhoneAuthCredential
    {
        private readonly PhoneAuthCredential _phoneAuthCredential;

        public PhoneAuthCredentialWrapper(PhoneAuthCredential phoneAuthCredential) : base(phoneAuthCredential)
        {
            _phoneAuthCredential = phoneAuthCredential;
        }

        public static explicit operator PhoneAuthCredential(PhoneAuthCredentialWrapper wrapper)
        {
            return wrapper._phoneAuthCredential;
        }
    }
}
