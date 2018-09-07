using System;
namespace Plugin.FirebaseAuth
{
    public interface IGoogleAuthProvider
    {
        string ProviderId { get; }
        IAuthCredential GetCredential(string idToken, string accessToken);
    }
}
