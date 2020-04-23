using System;
namespace Plugin.FirebaseAuth
{
    public class FacebookAuthProviderWrapper : IFacebookAuthProvider
    {
        public string ProviderId => Firebase.Auth.FacebookAuthProvider.Id;

        public string FacebookSignInMethod => Firebase.Auth.FacebookAuthProvider.SignInMethod;

        public IAuthCredential GetCredential(string accessToken)
        {
            var credential = Firebase.Auth.FacebookAuthProvider.GetCredential(accessToken);
            return new AuthCredentialWrapper(credential);
        }
    }
}
