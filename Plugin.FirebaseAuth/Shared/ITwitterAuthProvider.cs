using System;
namespace Plugin.FirebaseAuth
{
    public interface ITwitterAuthProvider
    {
        string ProviderId { get; }
        string TwitterSignInMethod { get; }
        IAuthCredential GetCredential(string token, string secret);
    }
}
