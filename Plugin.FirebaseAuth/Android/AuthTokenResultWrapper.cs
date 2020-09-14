using System;
using System.Collections.Generic;
using System.Linq;
using Firebase.Auth;

namespace Plugin.FirebaseAuth
{
    public class AuthTokenResultWrapper : IAuthTokenResult, IEquatable<AuthTokenResultWrapper>
    {
        private readonly GetTokenResult _getTokenResult;

        public AuthTokenResultWrapper(GetTokenResult getTokenResult)
        {
            _getTokenResult = getTokenResult ?? throw new ArgumentNullException(nameof(getTokenResult));
        }

        public string? Token => _getTokenResult.Token;

        public DateTimeOffset ExpirationDate =>
            new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero).AddSeconds(_getTokenResult.ExpirationTimestamp);

        public DateTimeOffset AuthDate =>
            new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero).AddSeconds(_getTokenResult.AuthTimestamp);

        public DateTimeOffset IssuedAtDate =>
            new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero).AddSeconds(_getTokenResult.IssuedAtTimestamp);

        public string? SignInProvider => _getTokenResult.SignInProvider;

        public string? SignInSecondFactor => _getTokenResult.SignInSecondFactor;

        public IDictionary<string, object?> Claims => _getTokenResult.Claims
            .ToDictionary(pair => pair.Key, pair => ValueConverter.Convert(pair.Value));

        public override bool Equals(object? obj)
        {
            return Equals(obj as AuthTokenResultWrapper);
        }

        public bool Equals(AuthTokenResultWrapper? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (GetType() != other.GetType()) return false;
            if (ReferenceEquals(_getTokenResult, other._getTokenResult)) return true;
            return _getTokenResult.Equals(other._getTokenResult);
        }

        public override int GetHashCode()
        {
            return _getTokenResult.GetHashCode();
        }
    }
}
