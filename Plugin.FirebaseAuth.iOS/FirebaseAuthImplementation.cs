using System;
using Firebase.Auth;

namespace Plugin.FirebaseAuth
{
    public class FirebaseAuthImplementation : IFirebaseAuth
    {
        public IAuth Instance => new AuthWrapper(Auth.DefaultInstance);

        public IAuth GetInstance(string appName)
        {
            var app = Firebase.Core.App.From(appName);
            return new AuthWrapper(Auth.From(app));
        }
    }
}