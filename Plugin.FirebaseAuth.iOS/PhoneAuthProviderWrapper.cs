using System;
using System.Threading.Tasks;
using Firebase.Auth;
using Foundation;
using ObjCRuntime;
using UIKit;

namespace Plugin.FirebaseAuth
{
    public class PhoneAuthProviderWrapper : IPhoneAuthProvider
    {
        public string ProviderId => PhoneAuthProvider.Id;

        public IPhoneAuthCredential GetCredential(string verificationId, string verificationCode)
        {
            var credential = PhoneAuthProvider.DefaultInstance.GetCredential(verificationId, verificationCode);
            return new PhoneAuthCredentialWrapper(credential);
        }

        public async Task<(IPhoneAuthCredential Credential, string VerificationCode)> VerifyPhoneNumberAsync(string phoneNumber)
        {
            try
            {
                var verificationCode = await PhoneAuthProvider.DefaultInstance.VerifyPhoneNumberAsync(phoneNumber, FirebaseAuth.VerifyingPhoneNumberAuthUIDelegate)
                                                              .ConfigureAwait(false);
                return (null, verificationCode);
            }
            catch (NSErrorException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }
    }
}
