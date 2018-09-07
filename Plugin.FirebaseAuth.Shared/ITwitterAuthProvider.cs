using System;
namespace Plugin.FirebaseAuth
{
    public interface ITwitterAuthProvider
    {
        string ProviderId { get; }
        IAuthCredential GetCredential(string token, string secret);
    }
}
