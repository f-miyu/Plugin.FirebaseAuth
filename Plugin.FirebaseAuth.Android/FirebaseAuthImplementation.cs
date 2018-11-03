using System;
using System.Threading.Tasks;
using Firebase;

namespace Plugin.FirebaseAuth
{
    public class FirebaseAuthImplementation : IFirebaseAuth
    {
        public IAuth Instance
        {
            get
            {
                Firebase.Auth.FirebaseAuth auth;
                if (string.IsNullOrEmpty(FirebaseAuth.DefaultAppName))
                {
                    auth = Firebase.Auth.FirebaseAuth.Instance;
                }
                else
                {
                    var app = FirebaseApp.GetInstance(FirebaseAuth.DefaultAppName);
                    auth = Firebase.Auth.FirebaseAuth.GetInstance(app);
                }
                return new AuthWrapper(auth);
            }
        }

        public IAuth GetInstance(string appName)
        {
            var app = FirebaseApp.GetInstance(appName);
            return new AuthWrapper(Firebase.Auth.FirebaseAuth.GetInstance(app));
        }
    }
}
