using System;
using Firebase.Auth;

namespace Plugin.FirebaseAuth
{
    public class OAuthProviderWrapper : IOAuthProvider
    {
        public IAuthCredential GetCredential(string providerId, string idToken, string? accessToken = null, string? rawNonce = null)
        {
            var credential = Firebase.Auth.OAuthProvider.GetCredential(providerId, idToken, rawNonce, accessToken);
            return new OAuthCredentialWrapper(credential);
        }
    }
}
