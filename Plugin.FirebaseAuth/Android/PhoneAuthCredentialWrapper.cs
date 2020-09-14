using System;
using Firebase.Auth;
namespace Plugin.FirebaseAuth
{
    public class PhoneAuthCredentialWrapper : AuthCredentialWrapper, IPhoneAuthCredential, IEquatable<PhoneAuthCredentialWrapper>
    {
        private readonly PhoneAuthCredential _phoneAuthCredential;

        public PhoneAuthCredentialWrapper(PhoneAuthCredential phoneAuthCredential) : base(phoneAuthCredential)
        {
            _phoneAuthCredential = phoneAuthCredential ?? throw new ArgumentNullException(nameof(phoneAuthCredential));
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as PhoneAuthCredentialWrapper);
        }

        public bool Equals(PhoneAuthCredentialWrapper? other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (GetType() != other.GetType()) return false;
            if (ReferenceEquals(_phoneAuthCredential, other._phoneAuthCredential)) return true;
            return _phoneAuthCredential.Equals(other._phoneAuthCredential);
        }

        public override int GetHashCode()
        {
            return _phoneAuthCredential.GetHashCode();
        }

        PhoneAuthCredential IPhoneAuthCredential.ToNative()
        {
            return _phoneAuthCredential;
        }
    }
}
