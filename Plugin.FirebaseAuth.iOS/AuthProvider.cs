using System;
using System.Collections.Concurrent;
using Firebase.Auth;

namespace Plugin.FirebaseAuth
{
    internal static class AuthProvider
    {
        private static ConcurrentDictionary<Auth, AuthWrapper> _auths = new ConcurrentDictionary<Auth, AuthWrapper>();

        public static AuthWrapper Auth => _auths.GetOrAdd(Firebase.Auth.Auth.DefaultInstance, key => new AuthWrapper(key));

        public static AuthWrapper GetAuth(string appName)
        {
            var app = Firebase.Core.App.From(appName);
            return _auths.GetOrAdd(Firebase.Auth.Auth.From(app), key => new AuthWrapper(key));
        }

        public static AuthWrapper GetAuth(Auth auth)
        {
            return _auths.GetOrAdd(auth, key => new AuthWrapper(key));
        }
    }
}
