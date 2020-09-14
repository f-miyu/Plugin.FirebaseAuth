using System;
using Firebase.Auth;

namespace Plugin.FirebaseAuth
{
    public class MultiFactorSessionWrapper : IMultiFactorSession, IEquatable<MultiFactorSessionWrapper>
    {
        private readonly MultiFactorSession _multiFactorSession;

        public MultiFactorSessionWrapper(MultiFactorSession multiFactorSession)
        {
            _multiFactorSession = multiFactorSession ?? throw new ArgumentNullException(nameof(multiFactorSession));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as MultiFactorSessionWrapper);
        }

        public bool Equals(MultiFactorSessionWrapper? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (GetType() != other.GetType()) return false;
            if (ReferenceEquals(_multiFactorSession, other._multiFactorSession)) return true;
            return _multiFactorSession.Equals(other._multiFactorSession);
        }

        public override int GetHashCode()
        {
            return _multiFactorSession.GetHashCode();
        }

        MultiFactorSession IMultiFactorSession.ToNative()
        {
            return _multiFactorSession;
        }
    }
}
