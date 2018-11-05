using System;
using Firebase.Auth;

namespace Plugin.FirebaseAuth
{
    public class FirebaseAuthImplementation : IFirebaseAuth
    {
        public IEmailAuthProvider EmailAuthProvider { get; } = new EmailAuthProviderWrapper();

        public IGoogleAuthProvider GoogleAuthProvider { get; } = new GoogleAuthProviderWrapper();

        public IFacebookAuthProvider FacebookAuthProvider { get; } = new FacebookAuthProviderWrapper();

        public ITwitterAuthProvider TwitterAuthProvider { get; } = new TwitterAuthProviderWrapper();

        public IGitHubAuthProvider GitHubAuthProvider { get; } = new GitHubAuthProviderWrapper();

        public IPhoneAuthProvider PhoneAuthProvider { get; } = new PhoneAuthProviderWrapper();

        public IOAuthProvider OAuthProvider { get; } = new OAuthProviderWrapper();

        public IAuth Instance => new AuthWrapper(Auth.DefaultInstance);

        public IAuth GetInstance(string appName)
        {
            var app = Firebase.Core.App.From(appName);
            return new AuthWrapper(Auth.From(app));
        }
    }
}