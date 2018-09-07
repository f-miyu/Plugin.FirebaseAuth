using System;
namespace Plugin.FirebaseAuth
{
    public interface IGitHubAuthProvider
    {
        string ProviderId { get; }
        IAuthCredential GetCredential(string token);
    }
}
