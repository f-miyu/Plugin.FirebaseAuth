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
                var app = FirebaseApp.GetInstance(FirebaseAuth.DefaultAppName);
                return new AuthWrapper(Firebase.Auth.FirebaseAuth.GetInstance(app));
            }
        }

        public IAuth GetInstance(string appName)
        {
            var app = FirebaseApp.GetInstance(appName);
            return new AuthWrapper(Firebase.Auth.FirebaseAuth.GetInstance(app));
        }
    }
}
