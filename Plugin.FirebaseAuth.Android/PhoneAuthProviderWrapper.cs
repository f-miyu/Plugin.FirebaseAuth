using System;
using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Java.Util.Concurrent;

namespace Plugin.FirebaseAuth
{
    public class PhoneAuthProviderWrapper : IPhoneAuthProvider
    {
        public string ProviderId => PhoneAuthProvider.ProviderId;

        private PhoneAuthProvider.ForceResendingToken _forceResendingToken;

        public IPhoneAuthCredential GetCredential(string verificationId, string smsCode)
        {
            var credential = PhoneAuthProvider.GetCredential(verificationId, smsCode);
            return new PhoneAuthCredentialWrapper(credential);
        }

        public async Task<(IPhoneAuthCredential Credential, string VerificationCode)> VerifyPhoneNumberAsync(string phoneNumber, bool forceResend = false)
        {
            var tcs = new TaskCompletionSource<(IPhoneAuthCredential Credential, string VerificationCode, PhoneAuthProvider.ForceResendingToken ForceResendingToken)>();
            var callbacks = new Callbacks(tcs);

            if (forceResend && _forceResendingToken != null)
            {
                PhoneAuthProvider.Instance.VerifyPhoneNumber(phoneNumber, FirebaseAuth.VerifyingPhoneNumberTimeout, TimeUnit.Seconds, FirebaseAuth.CurrentTopActivity, callbacks, _forceResendingToken);
            }
            else
            {
                PhoneAuthProvider.Instance.VerifyPhoneNumber(phoneNumber, FirebaseAuth.VerifyingPhoneNumberTimeout, TimeUnit.Seconds, FirebaseAuth.CurrentTopActivity, callbacks);
            }

            var (credential, verificationCode, forceResendingToken) = await tcs.Task.ConfigureAwait(false);

            _forceResendingToken = forceResendingToken;

            return (credential, verificationCode);
        }

        private class Callbacks : PhoneAuthProvider.OnVerificationStateChangedCallbacks
        {
            private TaskCompletionSource<(IPhoneAuthCredential Credential, string VerificationCode, PhoneAuthProvider.ForceResendingToken ForceResendingToken)> _tcs;

            public Callbacks(TaskCompletionSource<(IPhoneAuthCredential Credential, string VerificationCode, PhoneAuthProvider.ForceResendingToken ForceResendingToken)> tcs)
            {
                _tcs = tcs;
            }

            public override void OnVerificationCompleted(PhoneAuthCredential credential)
            {
                _tcs.SetResult((new PhoneAuthCredentialWrapper(credential), null, null));
            }

            public override void OnVerificationFailed(FirebaseException exception)
            {
                _tcs.SetException(ExceptionMapper.Map(exception));
            }

            public override void OnCodeSent(string verificationId, PhoneAuthProvider.ForceResendingToken forceResendingToken)
            {
                base.OnCodeSent(verificationId, forceResendingToken);

                _tcs.SetResult((null, verificationId, forceResendingToken));
            }

            public override void OnCodeAutoRetrievalTimeOut(string verificationId)
            {
                base.OnCodeAutoRetrievalTimeOut(verificationId);

                _tcs.SetResult((null, verificationId, null));
            }
        }
    }
}
