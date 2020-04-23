using System;
using Firebase.Auth;
namespace Plugin.FirebaseAuth
{
    public class PlayGamesAuthProviderWrapper : IPlayGamesAuthProvider
    {
        public string ProviderId => PlayGamesAuthProvider.ProviderId;

        public string PlayGamesSignInMethod => PlayGamesAuthProvider.PlayGamesSignInMethod;

        public IAuthCredential GetCredential(string serverAuthCode)
        {
            var credential = PlayGamesAuthProvider.GetCredential(serverAuthCode);
            return new AuthCredentialWrapper(credential);
        }
    }
}
