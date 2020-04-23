using System;
using Firebase.Auth;
namespace Plugin.FirebaseAuth
{
    public class AuthCredentialWrapper : IAuthCredential
    {
        private readonly AuthCredential _authCredential;

        public string Provider => _authCredential.Provider;

        public AuthCredentialWrapper(AuthCredential authCredential)
        {
            _authCredential = authCredential;
        }

        public static explicit operator AuthCredential(AuthCredentialWrapper wrapper)
        {
            return wrapper._authCredential;
        }
    }
}
