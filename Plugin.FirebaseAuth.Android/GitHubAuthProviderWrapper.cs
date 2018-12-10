using System;
using Firebase.Auth;
namespace Plugin.FirebaseAuth
{
    public class GitHubAuthProviderWrapper : IGitHubAuthProvider
    {
        public string ProviderId => GithubAuthProvider.ProviderId;

        public string GithubSignInMethod => GithubAuthProvider.GithubSignInMethod;

        public IAuthCredential GetCredential(string token)
        {
            var credential = GithubAuthProvider.GetCredential(token);
            return new AuthCredentialWrapper(credential);
        }
    }
}
