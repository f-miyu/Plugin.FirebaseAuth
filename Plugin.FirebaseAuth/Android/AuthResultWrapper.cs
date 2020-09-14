using System;
namespace Plugin.FirebaseAuth
{
    public class AuthResultWrapper : IAuthResult, IEquatable<AuthResultWrapper>
    {
        private readonly Firebase.Auth.IAuthResult _authResult;

        public AuthResultWrapper(Firebase.Auth.IAuthResult authResult)
        {
            _authResult = authResult ?? throw new ArgumentNullException(nameof(authResult));
        }

        public IAdditionalUserInfo? AdditionalUserInfo => _authResult.AdditionalUserInfo != null ? new AdditionalUserInfoWrapper(_authResult.AdditionalUserInfo) : null;

        public IUser? User => _authResult.User != null ? new UserWrapper(_authResult.User) : null;

        public IAuthCredential? Credential => _authResult.Credential != null ? AuthCredentialWrapperFactory.Create(_authResult.Credential) : null;

        public override bool Equals(object? obj)
        {
            return Equals(obj as AuthResultWrapper);
        }

        public bool Equals(AuthResultWrapper? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (GetType() != other.GetType()) return false;
            if (ReferenceEquals(_authResult, other._authResult)) return true;
            return _authResult.Equals(other._authResult);
        }

        public override int GetHashCode()
        {
            return _authResult.GetHashCode();
        }
    }
}
