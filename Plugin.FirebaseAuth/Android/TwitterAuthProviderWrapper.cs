using System;
using Firebase.Auth;
namespace Plugin.FirebaseAuth
{
    public class TwitterAuthProviderWrapper : ITwitterAuthProvider
    {
        public string ProviderId => TwitterAuthProvider.ProviderId;

        public string TwitterSignInMethod => TwitterAuthProvider.TwitterSignInMethod;

        public IAuthCredential GetCredential(string token, string secret)
        {
            var credential = TwitterAuthProvider.GetCredential(token, secret);
            return new AuthCredentialWrapper(credential);
        }
    }
}
