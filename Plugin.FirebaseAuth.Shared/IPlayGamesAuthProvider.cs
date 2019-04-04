using System;
namespace Plugin.FirebaseAuth
{
    public interface IPlayGamesAuthProvider
    {
        string ProviderId { get; }
        string PlayGamesSignInMethod { get; }
        IAuthCredential GetCredential(string serverAuthCode);
    }
}
