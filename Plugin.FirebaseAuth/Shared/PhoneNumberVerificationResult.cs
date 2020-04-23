using System;
namespace Plugin.FirebaseAuth
{
    public class PhoneNumberVerificationResult
    {
        public IPhoneAuthCredential Credential { get; }
        public string VerificationId { get; }

        public PhoneNumberVerificationResult(IPhoneAuthCredential credential, string verificationId)
        {
            Credential = credential;
            VerificationId = verificationId;
        }
    }
}
