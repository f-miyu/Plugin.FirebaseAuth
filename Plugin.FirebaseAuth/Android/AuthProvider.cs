using System;
using System.Collections.Concurrent;
using Firebase;

namespace Plugin.FirebaseAuth
{
    internal static class AuthProvider
    {
        private static ConcurrentDictionary<Firebase.Auth.FirebaseAuth, Lazy<AuthWrapper>> _auths = new ConcurrentDictionary<Firebase.Auth.FirebaseAuth, Lazy<AuthWrapper>>();

        public static AuthWrapper Auth { get; } = new AuthWrapper(Firebase.Auth.FirebaseAuth.Instance);

        public static AuthWrapper GetAuth(string appName)
        {
            var app = FirebaseApp.GetInstance(appName);
            return GetAuth(Firebase.Auth.FirebaseAuth.GetInstance(app));
        }

        public static AuthWrapper GetAuth(Firebase.Auth.FirebaseAuth auth)
        {
            if (auth == Firebase.Auth.FirebaseAuth.Instance)
            {
                return Auth;
            }
            return _auths.GetOrAdd(auth, key => new Lazy<AuthWrapper>(() => new AuthWrapper(key))).Value;
        }
    }
}
