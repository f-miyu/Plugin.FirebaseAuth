using System;
using Firebase.Auth;

namespace Plugin.FirebaseAuth
{
    public class FirebaseAuthImplementation : IFirebaseAuth
    {
        public IAuth Instance
        {
            get
            {
                Auth auth;
                if (string.IsNullOrEmpty(FirebaseAuth.DefaultAppName))
                {
                    auth = Auth.DefaultInstance;
                }
                else
                {
                    var app = Firebase.Core.App.From(FirebaseAuth.DefaultAppName);
                    auth = Auth.From(app);
                }
                return new AuthWrapper(auth);
            }
        }

        public IAuth GetInstance(string appName)
        {
            var app = Firebase.Core.App.From(appName);
            return new AuthWrapper(Auth.From(app));
        }
    }
}