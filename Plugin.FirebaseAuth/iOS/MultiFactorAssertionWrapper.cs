using System;
using Firebase.Auth;

namespace Plugin.FirebaseAuth
{
    public class MultiFactorAssertionWrapper : IMultiFactorAssertion, IEquatable<MultiFactorAssertionWrapper>
    {
        private readonly MultiFactorAssertion _multiFactorAssertion;

        public MultiFactorAssertionWrapper(MultiFactorAssertion multiFactorAssertion)
        {
            _multiFactorAssertion = multiFactorAssertion ?? throw new ArgumentNullException(nameof(multiFactorAssertion));
        }

        public string FactorId => _multiFactorAssertion.FactorId;

        public override bool Equals(object? obj)
        {
            return Equals(obj as MultiFactorAssertionWrapper);
        }

        public bool Equals(MultiFactorAssertionWrapper? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (GetType() != other.GetType()) return false;
            if (ReferenceEquals(_multiFactorAssertion, other._multiFactorAssertion)) return true;
            return _multiFactorAssertion.Equals(other._multiFactorAssertion);
        }

        public override int GetHashCode()
        {
            return _multiFactorAssertion.GetHashCode();
        }

        MultiFactorAssertion IMultiFactorAssertion.ToNative()
        {
            return _multiFactorAssertion;
        }
    }
}
