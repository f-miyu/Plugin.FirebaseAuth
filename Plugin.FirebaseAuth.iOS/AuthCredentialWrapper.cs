using System;
using Firebase.Auth;
namespace Plugin.FirebaseAuth
{
    public class AuthCredentialWrapper : IAuthCredential
    {
        internal AuthCredential AuthCredential { get; }

        public string Provider => AuthCredential.Provider;

        public AuthCredentialWrapper(AuthCredential authCredential)
        {
            AuthCredential = authCredential;
        }
    }
}
