using System;
using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Java.Util.Concurrent;
using Android.App;
using System.Diagnostics;

namespace Plugin.FirebaseAuth
{
    public class PhoneAuthProviderWrapper : IPhoneAuthProvider
    {
        public string ProviderId => PhoneAuthProvider.ProviderId;

        public IPhoneAuthCredential GetCredential(string verificationId, string smsCode)
        {
            var credential = PhoneAuthProvider.GetCredential(verificationId, smsCode);
            return new PhoneAuthCredentialWrapper(credential);
        }

        public Task<(IPhoneAuthCredential Credential, string VerificationCode)> VerifyPhoneNumberAsync(string phoneNumber)
        {
            var activity = FirebaseAuth.CurrentActivity ?? throw new NullReferenceException("current activity is null");

            var tcs = new TaskCompletionSource<(IPhoneAuthCredential Credential, string VerificationCode)>();
            var callbacks = new Callbacks(tcs);

            PhoneAuthProvider.Instance.VerifyPhoneNumber(phoneNumber, FirebaseAuth.VerifyingPhoneNumberTimeout, TimeUnit.Seconds, activity, callbacks);

            return tcs.Task;
        }

        private class Callbacks : PhoneAuthProvider.OnVerificationStateChangedCallbacks
        {
            private TaskCompletionSource<(IPhoneAuthCredential Credential, string VerificationCode)> _tcs;

            public Callbacks(TaskCompletionSource<(IPhoneAuthCredential Credential, string VerificationCode)> tcs)
            {
                _tcs = tcs;
            }

            public override void OnVerificationCompleted(PhoneAuthCredential credential)
            {
                _tcs.TrySetResult((new PhoneAuthCredentialWrapper(credential), null));
            }

            public override void OnVerificationFailed(FirebaseException exception)
            {
                _tcs.SetException(ExceptionMapper.Map(exception));
            }

            public override void OnCodeSent(string verificationId, PhoneAuthProvider.ForceResendingToken forceResendingToken)
            {
                base.OnCodeSent(verificationId, forceResendingToken);

                _tcs.TrySetResult((null, verificationId));
            }

            public override void OnCodeAutoRetrievalTimeOut(string verificationId)
            {
                base.OnCodeAutoRetrievalTimeOut(verificationId);

                _tcs.TrySetResult((null, verificationId));
            }
        }
    }
}
