using System;
using System.Threading.Tasks;
using Firebase.Auth;
using Foundation;

namespace Plugin.FirebaseAuth
{
    public class PhoneAuthProviderWrapper : IPhoneAuthProvider
    {
        public string ProviderId => PhoneAuthProvider.Id;

        public IPhoneAuthCredential GetCredential(IAuth auth, string verificationId, string verificationCode)
        {
            var wrapper = (AuthWrapper)auth;
            var credential = PhoneAuthProvider.From((Auth)wrapper).GetCredential(verificationId, verificationCode);
            return new PhoneAuthCredentialWrapper(credential);
        }

        public async Task<PhoneNumberVerificationResult> VerifyPhoneNumberAsync(IAuth auth, string phoneNumber)
        {
            try
            {
                var wrapper = (AuthWrapper)auth;
                var verificationId = await PhoneAuthProvider.From((Auth)wrapper)
                                                            .VerifyPhoneNumberAsync(phoneNumber, FirebaseAuth.VerifyingPhoneNumberAuthUIDelegate)
                                                            .ConfigureAwait(false);

                return new PhoneNumberVerificationResult(null, verificationId);
            }
            catch (NSErrorException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }
    }
}
