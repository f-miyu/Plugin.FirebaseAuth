using System;
using System.Collections.Concurrent;
using Firebase;

namespace Plugin.FirebaseAuth
{
    internal static class AuthProvider
    {
        private static ConcurrentDictionary<Firebase.Auth.FirebaseAuth, Lazy<AuthWrapper>> _auths = new ConcurrentDictionary<Firebase.Auth.FirebaseAuth, Lazy<AuthWrapper>>();

        public static AuthWrapper Auth => _auths.GetOrAdd(Firebase.Auth.FirebaseAuth.Instance, key => new Lazy<AuthWrapper>(() => new AuthWrapper(key))).Value;

        public static AuthWrapper GetAuth(string appName)
        {
            var app = FirebaseApp.GetInstance(appName);
            return _auths.GetOrAdd(Firebase.Auth.FirebaseAuth.GetInstance(app), key => new Lazy<AuthWrapper>(() => new AuthWrapper(key))).Value;
        }

        public static AuthWrapper GetAuth(Firebase.Auth.FirebaseAuth auth)
        {
            return _auths.GetOrAdd(auth, key => new Lazy<AuthWrapper>(() => new AuthWrapper(key))).Value;
        }
    }
}
