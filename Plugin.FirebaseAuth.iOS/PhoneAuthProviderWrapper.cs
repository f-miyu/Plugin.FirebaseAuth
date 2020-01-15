using System;
using System.Threading.Tasks;
using Firebase.Auth;
using Foundation;

namespace Plugin.FirebaseAuth
{
    public class PhoneAuthProviderWrapper : IPhoneAuthProvider
    {
        public string ProviderId => PhoneAuthProvider.Id;

        public string PhoneSignInMethod => PhoneAuthProvider.SignInMethod;

        public IPhoneAuthCredential GetCredential(IAuth auth, string verificationId, string verificationCode)
        {
            var wrapper = (AuthWrapper)auth;
            var credential = PhoneAuthProvider.Create((Auth)wrapper).GetCredential(verificationId, verificationCode);
            return new PhoneAuthCredentialWrapper(credential);
        }

        public Task<PhoneNumberVerificationResult> VerifyPhoneNumberAsync(IAuth auth, string phoneNumber)
        {
            return VerifyPhoneNumberAsync(auth, phoneNumber, default(TimeSpan));
        }

        public async Task<PhoneNumberVerificationResult> VerifyPhoneNumberAsync(IAuth auth, string phoneNumber, TimeSpan timeSpan)
        {
            try
            {
                var wrapper = (AuthWrapper)auth;
                var firebaseAuth = (Auth)wrapper;
                firebaseAuth.Settings.AppVerificationDisabledForTesting = false;

                var verificationId = await PhoneAuthProvider.Create(firebaseAuth)
                                                            .VerifyPhoneNumberAsync(phoneNumber, FirebaseAuth.VerifyingPhoneNumberAuthUIDelegate)
                                                            .ConfigureAwait(false);

                return new PhoneNumberVerificationResult(null, verificationId);
            }
            catch (NSErrorException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        public Task<PhoneNumberVerificationResult> VerifyPhoneNumberForTestingAsync(IAuth auth, string phoneNumber, string verificationCode)
        {
            return VerifyPhoneNumberForTestingAsync(auth, phoneNumber, verificationCode, default(TimeSpan));
        }

        public async Task<PhoneNumberVerificationResult> VerifyPhoneNumberForTestingAsync(IAuth auth, string phoneNumber, string verificationCode, TimeSpan timeout)
        {
            try
            {
                var wrapper = (AuthWrapper)auth;
                var firebaseAuth = (Auth)wrapper;
                firebaseAuth.Settings.AppVerificationDisabledForTesting = true;

                var verificationId = await PhoneAuthProvider.Create(firebaseAuth)
                                                            .VerifyPhoneNumberAsync(phoneNumber, FirebaseAuth.VerifyingPhoneNumberAuthUIDelegate)
                                                            .ConfigureAwait(false);

                var credential = GetCredential(auth, verificationId, verificationCode);

                return new PhoneNumberVerificationResult(credential, verificationId);
            }
            catch (NSErrorException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }
    }
}
