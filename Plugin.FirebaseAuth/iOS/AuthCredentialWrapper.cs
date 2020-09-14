using System;
using Firebase.Auth;
namespace Plugin.FirebaseAuth
{
    public class AuthCredentialWrapper : IAuthCredential, IEquatable<AuthCredentialWrapper>
    {
        private readonly AuthCredential _authCredential;

        public AuthCredentialWrapper(AuthCredential authCredential)
        {
            _authCredential = authCredential ?? throw new ArgumentNullException(nameof(authCredential));
        }

        public string Provider => _authCredential.Provider;

        public override bool Equals(object? obj)
        {
            return Equals(obj as AuthCredentialWrapper);
        }

        public bool Equals(AuthCredentialWrapper? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (GetType() != other.GetType()) return false;
            if (ReferenceEquals(_authCredential, other._authCredential)) return true;
            return _authCredential.Equals(other._authCredential);
        }

        public override int GetHashCode()
        {
            return _authCredential.GetHashCode();
        }

        AuthCredential IAuthCredential.ToNative()
        {
            return _authCredential;
        }
    }
}
