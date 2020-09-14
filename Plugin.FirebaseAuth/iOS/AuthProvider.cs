using System;
using System.Collections.Concurrent;
using Firebase.Auth;

namespace Plugin.FirebaseAuth
{
    internal static class AuthProvider
    {
        private static readonly ConcurrentDictionary<Auth, Lazy<AuthWrapper>> _auths = new ConcurrentDictionary<Auth, Lazy<AuthWrapper>>();

        public static AuthWrapper Auth { get; } = new AuthWrapper(Firebase.Auth.Auth.DefaultInstance);

        public static AuthWrapper GetAuth(string appName)
        {
            var app = Firebase.Core.App.From(appName);
            return GetAuth(Firebase.Auth.Auth.From(app));
        }

        public static AuthWrapper GetAuth(Auth auth)
        {
            if (auth == Firebase.Auth.Auth.DefaultInstance)
            {
                return Auth;
            }
            return _auths.GetOrAdd(auth, key => new Lazy<AuthWrapper>(() => new AuthWrapper(key))).Value;
        }
    }
}
