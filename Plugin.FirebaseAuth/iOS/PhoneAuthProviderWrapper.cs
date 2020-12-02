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

        public IPhoneAuthCredential GetCredential(string verificationId, string verificationCode)
        {
            return GetCredential(Auth.DefaultInstance!, verificationId, verificationCode);
        }

        public IPhoneAuthCredential GetCredential(IAuth auth, string verificationId, string verificationCode)
        {
            return GetCredential(auth.ToNative(), verificationId, verificationCode);
        }

        private IPhoneAuthCredential GetCredential(Auth auth, string verificationId, string verificationCode)
        {
            var credential = PhoneAuthProvider.Create(auth).GetCredential(verificationId, verificationCode);
            return new PhoneAuthCredentialWrapper(credential);
        }

        public Task<PhoneNumberVerificationResult> VerifyPhoneNumberAsync(string phoneNumber)
        {
            return VerifyPhoneNumberAsync(Auth.DefaultInstance!, phoneNumber);
        }

        public Task<PhoneNumberVerificationResult> VerifyPhoneNumberAsync(string phoneNumber, TimeSpan timeout)
        {
            return VerifyPhoneNumberAsync(Auth.DefaultInstance!, phoneNumber);
        }

        public Task<PhoneNumberVerificationResult> VerifyPhoneNumberAsync(IAuth auth, string phoneNumber)
        {
            return VerifyPhoneNumberAsync(auth.ToNative(), phoneNumber);
        }

        public Task<PhoneNumberVerificationResult> VerifyPhoneNumberAsync(IAuth auth, string phoneNumber, TimeSpan timeSpan)
        {
            return VerifyPhoneNumberAsync(auth.ToNative(), phoneNumber);
        }

        public Task<PhoneNumberVerificationResult> VerifyPhoneNumberAsync(string phoneNumber, IMultiFactorSession multiFactorSession)
        {
            return VerifyPhoneNumberAsync(Auth.DefaultInstance!, phoneNumber, multiFactorSession);
        }

        public Task<PhoneNumberVerificationResult> VerifyPhoneNumberAsync(string phoneNumber, IMultiFactorSession multiFactorSession, TimeSpan timeout, bool requiresSmsValidation)
        {
            return VerifyPhoneNumberAsync(Auth.DefaultInstance!, phoneNumber, multiFactorSession);
        }

        public Task<PhoneNumberVerificationResult> VerifyPhoneNumberAsync(IPhoneMultiFactorInfo phoneMultiFactorInfo, IMultiFactorSession multiFactorSession)
        {
            return VerifyPhoneNumberAsync(Auth.DefaultInstance!, phoneMultiFactorInfo, multiFactorSession);
        }

        public Task<PhoneNumberVerificationResult> VerifyPhoneNumberAsync(IPhoneMultiFactorInfo phoneMultiFactorInfo, IMultiFactorSession multiFactorSession, TimeSpan timeout, bool requiresSmsValidation)
        {
            return VerifyPhoneNumberAsync(Auth.DefaultInstance!, phoneMultiFactorInfo, multiFactorSession);
        }

        public Task<PhoneNumberVerificationResult> VerifyPhoneNumberAsync(IAuth auth, string phoneNumber, IMultiFactorSession multiFactorSession)
        {
            return VerifyPhoneNumberAsync(auth.ToNative(), phoneNumber, multiFactorSession);
        }

        public Task<PhoneNumberVerificationResult> VerifyPhoneNumberAsync(IAuth auth, string phoneNumber, IMultiFactorSession multiFactorSession, TimeSpan timeout, bool requiresSmsValidation)
        {
            return VerifyPhoneNumberAsync(auth.ToNative(), phoneNumber, multiFactorSession);
        }

        public Task<PhoneNumberVerificationResult> VerifyPhoneNumberAsync(IAuth auth, IPhoneMultiFactorInfo phoneMultiFactorInfo, IMultiFactorSession multiFactorSession)
        {
            return VerifyPhoneNumberAsync(auth.ToNative(), phoneMultiFactorInfo, multiFactorSession);
        }

        public Task<PhoneNumberVerificationResult> VerifyPhoneNumberAsync(IAuth auth, IPhoneMultiFactorInfo phoneMultiFactorInfo, IMultiFactorSession multiFactorSession, TimeSpan timeout, bool requiresSmsValidation)
        {
            return VerifyPhoneNumberAsync(auth.ToNative(), phoneMultiFactorInfo, multiFactorSession);
        }

        public Task<PhoneNumberVerificationResult> VerifyPhoneNumberForTestingAsync(string phoneNumber, string verificationCode)
        {
            return VerifyPhoneNumberForTestingAsync(Auth.DefaultInstance!, phoneNumber, verificationCode, default);
        }

        public Task<PhoneNumberVerificationResult> VerifyPhoneNumberForTestingAsync(string phoneNumber, string verificationCode, TimeSpan timeout)
        {
            return VerifyPhoneNumberForTestingAsync(Auth.DefaultInstance!, phoneNumber, verificationCode, timeout);
        }

        public Task<PhoneNumberVerificationResult> VerifyPhoneNumberForTestingAsync(IAuth auth, string phoneNumber, string verificationCode)
        {
            return VerifyPhoneNumberForTestingAsync(auth.ToNative(), phoneNumber, verificationCode, default);
        }

        public Task<PhoneNumberVerificationResult> VerifyPhoneNumberForTestingAsync(IAuth auth, string phoneNumber, string verificationCode, TimeSpan timeout)
        {
            return VerifyPhoneNumberForTestingAsync(auth.ToNative(), phoneNumber, verificationCode, default);
        }

        private async Task<PhoneNumberVerificationResult> VerifyPhoneNumberAsync(Auth auth, string phoneNumber)
        {
            try
            {
                if (auth.Settings != null)
                {
                    auth.Settings.AppVerificationDisabledForTesting = false;
                }

                var verificationId = await PhoneAuthProvider.Create(auth)
                    .VerifyPhoneNumberAsync(phoneNumber, FirebaseAuth.VerifyingPhoneNumberAuthUIDelegate)
                    .ConfigureAwait(false);

                return new PhoneNumberVerificationResult(null, verificationId);
            }
            catch (NSErrorException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        private async Task<PhoneNumberVerificationResult> VerifyPhoneNumberAsync(Auth auth, string phoneNumber, IMultiFactorSession multiFactorSession)
        {
            try
            {
                if (auth.Settings != null)
                {
                    auth.Settings.AppVerificationDisabledForTesting = false;
                }

                var verificationId = await PhoneAuthProvider.Create(auth)
                    .VerifyPhoneNumberAsync(phoneNumber, FirebaseAuth.VerifyingPhoneNumberAuthUIDelegate, multiFactorSession.ToNative())
                    .ConfigureAwait(false);

                return new PhoneNumberVerificationResult(null, verificationId);
            }
            catch (NSErrorException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        private async Task<PhoneNumberVerificationResult> VerifyPhoneNumberAsync(Auth auth, IPhoneMultiFactorInfo phoneMultiFactorInfo, IMultiFactorSession multiFactorSession)
        {
            try
            {
                if (auth.Settings != null)
                {
                    auth.Settings.AppVerificationDisabledForTesting = false;
                }

                var verificationId = await PhoneAuthProvider.Create(auth)
                    .VerifyPhoneNumberAsync(phoneMultiFactorInfo.ToNative(), FirebaseAuth.VerifyingPhoneNumberAuthUIDelegate, multiFactorSession.ToNative())
                    .ConfigureAwait(false);

                return new PhoneNumberVerificationResult(null, verificationId);
            }
            catch (NSErrorException e)
            {
                throw ExceptionMapper.Map(e);
            }
        }

        private async Task<PhoneNumberVerificationResult> VerifyPhoneNumberForTestingAsync(Auth auth, string phoneNumber, string verificationCode, TimeSpan timeout)
        {
            try
            {
                if (auth.Settings == null)
                {
                    auth.Settings = new AuthSettings();
                }
                auth.Settings.AppVerificationDisabledForTesting = true;

                var verificationId = await PhoneAuthProvider.Create(auth)
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
