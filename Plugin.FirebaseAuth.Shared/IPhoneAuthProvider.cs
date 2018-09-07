using System;
using System.Threading.Tasks;
namespace Plugin.FirebaseAuth
{
    public interface IPhoneAuthProvider
    {
        string ProviderId { get; }
        IPhoneAuthCredential GetCredential(string verificationId, string verificationCode);
        Task<(IPhoneAuthCredential Credential, string VerificationCode)> VerifyPhoneNumberAsync(string phoneNumber, bool forceResend = false);
    }
}
