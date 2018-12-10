using System;
using Firebase.Auth;
namespace Plugin.FirebaseAuth
{
    public class GoogleAuthProviderWrapper : IGoogleAuthProvider
    {
        public string ProviderId => GoogleAuthProvider.ProviderId;

        public string GoogleSignInMethod => GoogleAuthProvider.GoogleSignInMethod;

        public IAuthCredential GetCredential(string idToken, string accessToken)
        {
            var credential = GoogleAuthProvider.GetCredential(idToken, accessToken);
            return new AuthCredentialWrapper(credential);
        }
    }
}
