using System;
using System.Collections.Generic;
using Firebase.Auth;

namespace Plugin.FirebaseAuth
{
    public class AuthTokenResultWrapper : IAuthTokenResult, IEquatable<AuthTokenResultWrapper>
    {
        private readonly AuthTokenResult _authTokenResult;

        public AuthTokenResultWrapper(AuthTokenResult authTokenResult)
        {
            _authTokenResult = authTokenResult ?? throw new ArgumentNullException(nameof(authTokenResult));
        }

        public string? Token => _authTokenResult.Token;

        public DateTimeOffset ExpirationDate =>
            new DateTimeOffset(2001, 1, 1, 0, 0, 0, TimeSpan.Zero).AddSeconds(_authTokenResult.ExpirationDate.SecondsSinceReferenceDate);

        public DateTimeOffset AuthDate =>
            new DateTimeOffset(2001, 1, 1, 0, 0, 0, TimeSpan.Zero).AddSeconds(_authTokenResult.AuthDate.SecondsSinceReferenceDate);

        public DateTimeOffset IssuedAtDate =>
            new DateTimeOffset(2001, 1, 1, 0, 0, 0, TimeSpan.Zero).AddSeconds(_authTokenResult.IssuedAtDate.SecondsSinceReferenceDate);

        public string? SignInProvider => _authTokenResult.SignInProvider;

        public string? SignInSecondFactor => _authTokenResult.SignInSecondFactor;

        public IDictionary<string, object?> Claims
        {
            get
            {
                var claims = new Dictionary<string, object?>();
                foreach (var (key, value) in _authTokenResult.Claims)
                {
                    claims[key.ToString()] = ValueConverter.Convert(value);
                }
                return claims;
            }
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as AuthTokenResultWrapper);
        }

        public bool Equals(AuthTokenResultWrapper? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (GetType() != other.GetType()) return false;
            if (ReferenceEquals(_authTokenResult, other._authTokenResult)) return true;
            return _authTokenResult.Equals(other._authTokenResult);
        }

        public override int GetHashCode()
        {
            return _authTokenResult.GetHashCode();
        }
    }
}
