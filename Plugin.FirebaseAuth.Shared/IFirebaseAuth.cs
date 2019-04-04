using System;

namespace Plugin.FirebaseAuth
{
    public interface IFirebaseAuth
    {
        IEmailAuthProvider EmailAuthProvider { get; }
        IGoogleAuthProvider GoogleAuthProvider { get; }
        IFacebookAuthProvider FacebookAuthProvider { get; }
        ITwitterAuthProvider TwitterAuthProvider { get; }
        IGitHubAuthProvider GitHubAuthProvider { get; }
        IPhoneAuthProvider PhoneAuthProvider { get; }
        IOAuthProvider OAuthProvider { get; }
        IPlayGamesAuthProvider PlayGamesAuthProvider { get; }
        IAuth Instance { get; }
        IAuth GetInstance(string appName);
    }
}
