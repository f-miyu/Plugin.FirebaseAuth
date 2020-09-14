using System;
namespace Plugin.FirebaseAuth
{
    public class PhoneNumberVerificationResult
    {
        public PhoneNumberVerificationResult(IPhoneAuthCredential? credential, string? verificationId)
        {
            Credential = credential;
            VerificationId = verificationId;
        }

        public IPhoneAuthCredential? Credential { get; }
        public string? VerificationId { get; }
    }
}
