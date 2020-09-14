using System;
using Firebase.Auth;

namespace Plugin.FirebaseAuth
{
    public class MultiFactorInfoWrapper : IMultiFactorInfo, IEquatable<MultiFactorInfoWrapper>
    {
        private readonly MultiFactorInfo _multiFactorInfo;

        public MultiFactorInfoWrapper(MultiFactorInfo multiFactorInfo)
        {
            _multiFactorInfo = multiFactorInfo ?? throw new ArgumentNullException(nameof(multiFactorInfo));
        }

        public string FactorIdKey => MultiFactorInfo.FactorIdKey;

        public string Uid => _multiFactorInfo.Uid;

        public string? DisplayName => _multiFactorInfo.DisplayName;

        public DateTimeOffset EnrollmentDate => new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero).AddSeconds(_multiFactorInfo.EnrollmentTimestamp);

        public string FactorId => _multiFactorInfo.FactorId;

        public override bool Equals(object? obj)
        {
            return Equals(obj as MultiFactorInfoWrapper);
        }

        public bool Equals(MultiFactorInfoWrapper? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (GetType() != other.GetType()) return false;
            if (ReferenceEquals(_multiFactorInfo, other._multiFactorInfo)) return true;
            return _multiFactorInfo.Equals(other._multiFactorInfo);
        }

        public override int GetHashCode()
        {
            return _multiFactorInfo.GetHashCode();
        }

        MultiFactorInfo IMultiFactorInfo.ToNative()
        {
            return _multiFactorInfo;
        }
    }
}
