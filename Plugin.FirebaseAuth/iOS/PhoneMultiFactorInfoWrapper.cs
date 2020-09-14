using System;
using Firebase.Auth;

namespace Plugin.FirebaseAuth
{
    public class PhoneMultiFactorInfoWrapper : MultiFactorInfoWrapper, IPhoneMultiFactorInfo, IEquatable<PhoneMultiFactorInfoWrapper>
    {
        private readonly PhoneMultiFactorInfo _phoneMultiFactorInfo;

        public PhoneMultiFactorInfoWrapper(PhoneMultiFactorInfo phoneMultiFactorInfo) : base(phoneMultiFactorInfo)
        {
            _phoneMultiFactorInfo = phoneMultiFactorInfo ?? throw new ArgumentNullException(nameof(phoneMultiFactorInfo));
        }

        public string PhoneNumber => _phoneMultiFactorInfo.PhoneNumber;

        public override bool Equals(object? obj)
        {
            return Equals(obj as PhoneMultiFactorInfoWrapper);
        }

        public bool Equals(PhoneMultiFactorInfoWrapper? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (GetType() != other.GetType()) return false;
            if (ReferenceEquals(_phoneMultiFactorInfo, other._phoneMultiFactorInfo)) return true;
            return _phoneMultiFactorInfo.Equals(other._phoneMultiFactorInfo);
        }

        public override int GetHashCode()
        {
            return _phoneMultiFactorInfo.GetHashCode();
        }

        PhoneMultiFactorInfo IPhoneMultiFactorInfo.ToNative()
        {
            return _phoneMultiFactorInfo;
        }
    }
}
