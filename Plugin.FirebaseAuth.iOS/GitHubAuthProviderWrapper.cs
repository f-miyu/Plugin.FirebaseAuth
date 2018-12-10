using System;
using Firebase.Auth;
namespace Plugin.FirebaseAuth
{
    public class GitHubAuthProviderWrapper : IGitHubAuthProvider
    {
        public string ProviderId => GitHubAuthProvider.Id;

        public string GithubSignInMethod => GitHubAuthProvider.SignInMethod;

        public IAuthCredential GetCredential(string token)
        {
            var credential = GitHubAuthProvider.GetCredential(token);
            return new AuthCredentialWrapper(credential);
        }
    }
}
