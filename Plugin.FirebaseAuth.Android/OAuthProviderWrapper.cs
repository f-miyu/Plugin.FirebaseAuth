using System;
using Firebase.Auth;
namespace Plugin.FirebaseAuth
{
    public class OAuthProviderWrapper : IOAuthProvider
    {
        public IAuthCredential GetCredential(string providerId, string idToken, string accessToken)
        {
            var credential = OAuthProvider.GetCredential(providerId, idToken, accessToken);
            return new AuthCredentialWrapper(credential);
        }
    }
}
