using System;
using Firebase.Auth;

namespace Plugin.FirebaseAuth
{
    public class OAuthCredentialWrapper : AuthCredentialWrapper, IOAuthCredential, IEquatable<OAuthCredentialWrapper>
    {
        private readonly OAuthCredential _oAuthCredential;

        public OAuthCredentialWrapper(OAuthCredential oAuthCredential) : base(oAuthCredential)
        {
            _oAuthCredential = oAuthCredential ?? throw new ArgumentNullException(nameof(oAuthCredential));
        }

        public string AccessToken => _oAuthCredential.AccessToken;

        public string IdToken => _oAuthCredential.IdToken;

        public string Secret => _oAuthCredential.Secret;

        public override bool Equals(object? obj)
        {
            return Equals(obj as OAuthCredentialWrapper);
        }

        public bool Equals(OAuthCredentialWrapper? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (GetType() != other.GetType()) return false;
            if (ReferenceEquals(_oAuthCredential, other._oAuthCredential)) return true;
            return _oAuthCredential.Equals(other._oAuthCredential);
        }

        public override int GetHashCode()
        {
            return _oAuthCredential.GetHashCode();
        }
    }
}
