using System;
namespace Plugin.FirebaseAuth
{
    public interface IGitHubAuthProvider
    {
        string ProviderId { get; }
        string GithubSignInMethod { get; }
        IAuthCredential GetCredential(string token);
    }
}
