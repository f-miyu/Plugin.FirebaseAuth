using System;
using Firebase.Auth;

namespace Plugin.FirebaseAuth
{
    internal static class AuthCredentialWrapperFactory
    {
        public static IAuthCredential Create(AuthCredential authCredential)
        {
            return authCredential switch
            {
                OAuthCredential oAuthCredential => new OAuthCredentialWrapper(oAuthCredential),
                PhoneAuthCredential phoneAuthCredential => new PhoneAuthCredentialWrapper(phoneAuthCredential),
                _ => new AuthCredentialWrapper(authCredential)
            };
        }
    }
}
