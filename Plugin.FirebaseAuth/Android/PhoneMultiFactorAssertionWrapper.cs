using System;
using Firebase.Auth;

namespace Plugin.FirebaseAuth
{
    public class PhoneMultiFactorAssertionWrapper : MultiFactorAssertionWrapper, IPhoneMultiFactorAssertion, IEquatable<PhoneMultiFactorAssertionWrapper>
    {
        private readonly PhoneMultiFactorAssertion _phoneMultiFactorAssertion;

        public PhoneMultiFactorAssertionWrapper(PhoneMultiFactorAssertion phoneMultiFactorAssertion) : base(phoneMultiFactorAssertion)
        {
            _phoneMultiFactorAssertion = phoneMultiFactorAssertion ?? throw new ArgumentNullException(nameof(phoneMultiFactorAssertion));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as PhoneMultiFactorAssertionWrapper);
        }

        public bool Equals(PhoneMultiFactorAssertionWrapper? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (GetType() != other.GetType()) return false;
            if (ReferenceEquals(_phoneMultiFactorAssertion, other._phoneMultiFactorAssertion)) return true;
            return _phoneMultiFactorAssertion.Equals(other._phoneMultiFactorAssertion);
        }

        public override int GetHashCode()
        {
            return _phoneMultiFactorAssertion.GetHashCode();
        }
    }
}
