using System;
using System.Collections.Concurrent;
using Firebase.Auth;

namespace Plugin.FirebaseAuth
{
    internal static class AuthProvider
    {
        private static readonly ConcurrentDictionary<Auth, Lazy<AuthWrapper>> _auths = new ConcurrentDictionary<Auth, Lazy<AuthWrapper>>();

        public static AuthWrapper Auth => _auths.GetOrAdd(Firebase.Auth.Auth.DefaultInstance, key => new Lazy<AuthWrapper>(() => new AuthWrapper(key))).Value;

        public static AuthWrapper GetAuth(string appName)
        {
            var app = Firebase.Core.App.From(appName);
            return _auths.GetOrAdd(Firebase.Auth.Auth.From(app), key => new Lazy<AuthWrapper>(() => new AuthWrapper(key))).Value;
        }

        public static AuthWrapper GetAuth(Auth auth)
        {
            return _auths.GetOrAdd(auth, key => new Lazy<AuthWrapper>(() => new AuthWrapper(key))).Value;
        }
    }
}
