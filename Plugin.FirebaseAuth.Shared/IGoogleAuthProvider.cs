using System;
namespace Plugin.FirebaseAuth
{
    public interface IGoogleAuthProvider
    {
        string ProviderId { get; }
        string GoogleSignInMethod { get; }
        IAuthCredential GetCredential(string idToken, string accessToken);
    }
}
